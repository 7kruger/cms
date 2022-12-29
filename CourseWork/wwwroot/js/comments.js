let comments = document.querySelector("#comments");

loadComments();

document.querySelector("#sendComment").addEventListener("click", () => {
    addComment();
});

async function loadComments() {

    clearComments();
    let srcId = document.querySelector("#srcId").value;

    try {
        const response = await fetch(`/api/loadcomments?id=${srcId}`, {
            method: "get",
            headers: {
                "content-type": "application/json"
            }
        });

        if (response.ok) {

            const data = await response.json();

            data.forEach(index => {
                //мне самому плохо от этого франкенштейна

                let div = document.createElement("div");
                let small = document.createElement("small");
                let strong = document.createElement("strong");
                div.setAttribute("class", "list-group-item list-group-item-light comment");
                let p = document.createElement("p");

                let d = new Date(Date.parse(index.date)).toUTCString();
                const date = getDate(d);

                strong.textContent = `Автор - "${index.userName}"     ${date}`;
                small.appendChild(strong);
                div.appendChild(small);
                p.textContent = `${index.content}`;
                div.appendChild(p);

                comments.appendChild(div);
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Не удалось загрузить комментарии',
            })
        }
    } catch (e) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
        })
    }   
}

async function addComment() {

    let srcId = document.querySelector("#srcId").value;
    let comment = document.querySelector("#comment").value;

    const formData = new FormData();
    formData.append("id", srcId);
    formData.append("content", comment);

    try {
        const response = await fetch("/api/addcomment", {
            method: "post",
            body: formData
        });

        if (response.ok) {
            document.getElementById("comment").value = "";
            loadComments();
        } else {
            Swal.fire({
                html: "<h1>Не удалось добавить комментарий</h1>"
            });
        }
    } catch (e) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
        })
    }

    
    
}

function clearComments() {
    var container = document.querySelector("#comments");

    while (container.firstChild) {
        container.removeChild(container.firstChild);
    }
}

function getDate(date) {

    let newDate = "";

    for (var i = 0; i < date.length - 7; i++) {
        newDate += date[i]
    }

    return newDate;
}