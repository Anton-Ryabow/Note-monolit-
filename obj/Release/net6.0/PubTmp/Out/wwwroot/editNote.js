window.addEventListener("load", async () => {
    const headers = {
        'Access-Control-Allow-Credentials': 'true',
    };
    let response = await fetch("https://localhost:7111/getNote", {
        method: "GET",
        headers,
        credentials: "include",
    });

    if (response.ok === true) {
        let result = await response.json();

        document.getElementsByName("title")[0].setAttribute("value", result.title);
        document.getElementsByName("content")[0].innerHTML = result.content;
    }
    else {
        const error = await response.json();
        alert(error.message);
        return;
    }
});

document.querySelector("form").addEventListener("submit", async (event) => {
    event.preventDefault();

    let title = document.getElementsByName("title")[0].value;
    let content = document.getElementsByName("content")[0].value;

    if (!(title && content)) {
        alert("Название и содержание не должны быть пустыми!");
        return;
    }

    let form = new FormData();

    form.append("id", getCookie(document.cookie.replaceAll(' ', ''), "NoteId"));
    form.append("title", title);
    form.append("content", content);

    const headers = {
        'Access-Control-Allow-Credentials': 'true',
    };
    let response = await fetch("https://localhost:7111/editNote", {
        method: "POST",
        headers,
        body: form,
        credentials: "include",
    });

    if (response.ok === true) {
        let result = await response.json();
        alert(result.message);
        window.location.replace("/notes");
    }
    else {
        const error = await response.json();
        alert(error.message);
        return;
    }
});

function getCookie(cooks, cookieName) {
    const cookies = cooks.split(";");
    for (c of cookies) {
        const parts = c.split("=");
        if (parts[0] === cookieName) return parts[1];
    }
}