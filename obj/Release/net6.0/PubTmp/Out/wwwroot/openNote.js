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
        document.querySelector("h3").innerHTML = result.title;
        document.querySelector("textarea").innerHTML = result.content;
        document.querySelector("title").innerHTML = result.title;
    }
    else {
        const error = await response.json();
        alert(error.message);
        return;
    }
});

let change = document.getElementsByClassName("button-change")[0]
change.onclick = () => {
    window.location.replace("/edit");
}

function getCookie(cooks, cookieName) {
    const cookies = cooks.split(";");
    for (c of cookies) {
        const parts = c.split("=");
        if (parts[0] === cookieName) return parts[1];
    }
}