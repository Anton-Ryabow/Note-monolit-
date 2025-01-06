using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Note;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddAuthentication("Cookies")
    .AddCookie(options => { 
        options.LoginPath = "/loginPath";
        options.Cookie.HttpOnly = false;
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationContextSqlite>();

var app = builder.Build();

app.UseStaticFiles();
app.UseCors(builder => builder.WithOrigins("https://localhost:7111")
                             .AllowCredentials()
                             .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", (HttpContext context) =>
{
    string? _count;

    if (context.Request.Cookies.TryGetValue("CountOfNotes", out _count))
    {
        if (int.Parse(_count) > 0)
            context.Response.Redirect("/notes.html");
        else
            context.Response.Redirect("/empty.html");
    }
    else
    {
        context.Response.Redirect("/index.html");
    }
});
app.MapGet("/loginPath", (HttpContext context) => context.Response.Redirect("/login.html"));
app.MapGet("/empty", [Authorize] (HttpContext context) => context.Response.Redirect("/empty.html"));
app.MapGet("/notes", [Authorize] (HttpContext context) => context.Response.Redirect("/notes.html"));
app.MapGet("/openNote", [Authorize] (HttpContext context) => context.Response.Redirect("/openNote.html"));


app.MapPost("/login", async (HttpContext context, ApplicationContextSqlite db) =>
{
    IFormCollection form = context.Request.Form;

    string? login = form["login"];
    string? password = form["password"];

    User? user = db.Users.Include(u => u.Notes).FirstOrDefault(u => u.Login == login && u.Password == password.GetHashPass());

    if (user == null)
        return Results.BadRequest(new { message = "Неверно указаны логин или пароль! (C#)" });

    context.Response.Cookies.Append("CountOfNotes", user.Notes.Count().ToString());

    List<Claim> claims = new List<Claim>()
    {
        new Claim("Id", user.Id.ToString()),
        new Claim("Name", user.Name)
    };

    ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");
    await context.SignInAsync("Cookies", new ClaimsPrincipal(identity));

    return Results.Ok(new { message = $"Пользователь авторизован! (C#)" });
});

app.MapPost("/registration", (HttpContext context, ApplicationContextSqlite db) =>
{
    IFormCollection form = context.Request.Form;

    User user = new User(form["name"], form["lastname"], form["email"], form["login"], form["password"].GetHashPass());

    db.Users.Add(user);
    db.SaveChanges();

    return Results.Ok(new { message = "Пользователь успешно зарегестрирован! (C#)" });
});

app.MapPost("/logout", (HttpContext context) =>
{
    context.Response.Cookies.Delete("CountOfNotes");
    context.Response.Cookies.Delete("NoteId");
    context.SignOutAsync("Cookies");
    return Results.Ok(new { message = "Пользователь вышел из аккаунта! (C#)" });
});

app.MapPost("/addNote", (HttpContext context, ApplicationContextSqlite db) =>
{
    IFormCollection form = context.Request.Form;

    string title = form["title"];
    string content = form["content"];

    int userId = Int32.Parse(context.User.FindFirst("Id").Value);

    User? user = db.Users.FirstOrDefault(u => u.Id == userId);

    if (user == null)
    {
        return Results.BadRequest(new { message = "Пользователь не найден!" });
    }
    try
    {
        Note.Note note = new(title, content, DateOnly.FromDateTime(DateTime.Now), userId, user);
        user.Notes.Add(note);
        db.SaveChanges();
    }
    catch
    {
        return Results.BadRequest(new { message = "Что-то не сохранилось в блоке try" });
    }

    int count = int.Parse(context.Request.Cookies["CountOfNotes"]) + 1;
    context.Response.Cookies.Append("CountOfNotes", count.ToString());

    return Results.Ok(new { message = "Заметка сохранена!" });
});

app.MapGet("/getNotes", (HttpContext context, ApplicationContextSqlite db) =>
{
    int userId = Int32.Parse(context.User.FindFirst("Id").Value);

    User? user = db.Users.Include(u => u.Notes).FirstOrDefault(u => u.Id == userId);

    if (user == null)
    {
        return Results.BadRequest(new { message = "Пользователь не найден!" });
    }

    return Results.Json(user.Notes.Select(n => new
    {
        n.Id,
        n.Title,
        n.Content,
        date = n.Date.ToString()
    }));
});

app.MapGet("/getNote", (HttpContext context, ApplicationContextSqlite db) =>
{
    int noteId = Int32.Parse(context.Request.Cookies["NoteId"]);

    var note = db.Notes.FirstOrDefault(n => n.Id == noteId);

    if (note == null)
    {
        return Results.BadRequest(new { message = "Заметка не найдена!" });
    }

    return Results.Json(new
    {
        note.Id,
        note.Title,
        note.Content,
    });
});

app.MapPost("/editNote", (HttpContext context, ApplicationContextSqlite db) =>
{
    IFormCollection form = context.Request.Form;

    int noteId = int.Parse(form["id"]);
    string? title = form["title"];
    string? content = form["content"];

    var note = db.Notes.FirstOrDefault(db => db.Id == noteId);

    if ( note == null )
    {
        return Results.BadRequest(new { message = "Заметка не найдена!" });
    }

    note.Title = title;
    note.Content = content;
    note.Date = DateOnly.FromDateTime(DateTime.Now);

    db.SaveChanges();

    context.Response.Cookies.Delete("NoteId");
    return Results.Ok(new { message = "Изменения учтены!" });
});

app.MapGet("/deleteNote", (HttpContext context, ApplicationContextSqlite db) =>
{
    int noteId = int.Parse(context.Request.Cookies["NoteId"]);

    var note = db.Notes.FirstOrDefault(n => n.Id == noteId);

    if (note == null)
    {
        return Results.BadRequest(new { message = "Заметка не найдена!" });
    }

    db.Notes.Remove(note);
    db.Backup.Add(new Backup()
    {
        Id = note.Id,
        Title = note.Title,
        Text = note.Content
    });
    db.SaveChanges();

    int count = int.Parse(context.Request.Cookies["CountOfNotes"]) - 1;
    context.Response.Cookies.Append("CountOfNotes", count.ToString());
    context.Response.Cookies.Delete("NoteId");

    return Results.Ok(new { message = "Заметка удалена!" });
});

app.Run();
