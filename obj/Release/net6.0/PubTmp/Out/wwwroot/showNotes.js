let notes;
let notesToPrint = [];

function getCookie(cooks, cookieName) {
    const cookies = cooks.split(";");
    for (c of cookies) {
        const parts = c.split("=");
        if (parts[0] === cookieName) return parts[1];
    }
}

window.addEventListener("load", async () => {
    const headers = {
        'Access-Control-Allow-Credentials': 'true',
    };

    let response = await fetch("https://localhost:7111/getNotes", {
        method: "GET",
        headers,
        credentials: "include",
    });

    if (response.ok === true) {

        let result = await response.json();
        notes = result;
    }
    else {
        const error = await response.json();
        alert(error.message);
        return;
    }

    for (let i = 0; i < notes.length; i++) {
        let note = document.createElement("div");
        let titleContainer = document.createElement("div");
        let textContainer = document.createElement("div");
        let title = document.createElement("h3");
        let date = document.createElement("h3");
        let text = document.createElement("p");

        note.className = "note";
        titleContainer.className = "note__title";
        textContainer.className = "note__text";

        title.innerHTML = notes[i].title;
        date.innerHTML = notes[i].date;
        text.innerHTML = notes[i].content.substring(0, 320) + '...';

        titleContainer.append(title);
        titleContainer.append(date);
        textContainer.append(text);
        note.append(titleContainer);
        note.append(textContainer);

        note.iddb = notes[i].id;

        notesToPrint.push(note);
    }

    console.log(notes);
    console.log(notesToPrint);

    for (let note of notesToPrint) {
        document.querySelector("main").append(note);
        note.addEventListener("click", showNote);
    }
})

async function showNote() {
    document.cookie = `NoteId=${this.iddb}`;

    window.location.replace("/openNote");
}