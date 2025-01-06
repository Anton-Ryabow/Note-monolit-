let deleteButton = document.getElementsByClassName("button-delete")[0];

deleteButton.addEventListener("click", async () => {
    const headers = {
        'Access-Control-Allow-Credentials': 'true',
    };
    let response = await fetch("https://localhost:7111/deleteNote", {
        method: "GET",
        headers,
        credentials: "include",
    });

    if (response.ok === true) {
        let result = await response.json();
        alert(result.message);

        let countOfNotes = Number(getCookie(document.cookie, "CountOfNotes"));
        if (countOfNotes === 0) {
            window.location.replace("/empty");
            return;
        }

        window.location.replace("/notes");
    }
    else {
        const error = await response.json();
        alert(error.message);
        return;
    }
});

function getCookie(cooks, cookieName) {
    let cookies = cooks.replaceAll(" ", "");
    cookies = cookies.split(";");
    for (c of cookies) {
        const parts = c.split("=");
        if (parts[0] === cookieName) return parts[1];
    }
}