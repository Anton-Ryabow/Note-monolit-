//let pic = document.getElementById("pic");

//if (document.cookie) {

//    if (document.getElementById("logout")) {
//        document.getElementById("logout").remove();
//    }

//    let button = document.createElement("button");
//    button.textContent = "Logout";
//    button.id = "logout";
//    document.body.append(button)

//    button.addEventListener("click", async () => {
//        const headers = {
//            'Access-Control-Allow-Credentials': 'true',
//        };

//        let response = await fetch("https://localhost:7111/logout", {
//            method: "POST",
//            headers,
//            credentials: "include",
//        });

//        if (response.ok === true) {
//            let result = await response.json();
//            alert(result.message);
//            window.location.replace("/loginPath");
//        }
//        else {
//            const error = await response.json();
//            alert(error.message);
//        }
//    });
//}
//else {
//    if (document.getElementById("logout")) {
//        document.getElementById("logout").remove();
//    }
//}

//if (document.cookie) {
//    if (document.getElementById("logout_window")) {
//        document.getElementById("logout_window").remove();
//    }

//    let div = document.createElement("div");
//    let name = document.createElement("h3");
//    let button = document.createElement("button");

//    name.innerText = "Name";
//    button.textContent = "Выйти";

//    div.className = "logout_window";
//    name.className = "logout_name";
//    button.className = "logout_button";

//    div.append(name);
//    div.append(button);
//    document.body.append(div);

//    button.addEventListener("click", async () => {
//        const headers = {
//            'Access-Control-Allow-Credentials': 'true',
//        };

//        let response = await fetch("https://localhost:7111/logout", {
//            method: "POST",
//            headers,
//            credentials: "include",
//        });

//        if (response.ok === true) {
//            let result = await response.json();
//            alert(result.message);
//            window.location.replace("/loginPath");
//        }
//        else {
//            const error = await response.json();
//            alert(error.message);
//        }
//    })
//}
//else {
//    if (document.getElementById("logout_window")) {
//        document.getElementById("logout_window").remove();
//    }
//}

let picture = document.getElementById("pic");

if (document.cookie) {
    picture.setAttribute("href", "#");

    picture.onclick = async () => {
        const headers = {
            'Access-Control-Allow-Credentials': 'true',
        };

        let response = await fetch("https://localhost:7111/logout", {
            method: "POST",
            headers,
            credentials: "include",
        });

        if (response.ok === true) {
            let result = await response.json();
            alert(result.message);
            window.location.replace("/loginPath");
        }
        else {
            const error = await response.json();
            alert(error.message);
        }
    }
}
else {
    picture.setAttribute("href", "login.html");
    picture.onclick = null;
}

/// Это в отдельный скрипт файл и его везде, где есть этот


let shild = document.getElementsByClassName("logo")[0];

if (document.cookie) {
    shild.setAttribute("href", "#");
    shild.onclick = () => {
        
        if (Number(getCookie(document.cookie, "CountOfNotes")) > 0) {
            window.location.replace("/notes");
        }
        else {
            window.location.replace("/empty");
        }
    }
}
else {
    shild.setAttribute("href", "index.html");
    shild.onclick = null;
}

function getCookie(cooks, cookieName) {
    const cookies = cooks.split(";");
    for (c of cookies) {
        const parts = c.split("=");
        if (parts[0] === cookieName) return parts[1];
    }
}