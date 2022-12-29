let comments = document.querySelector("#comments");

loadComments();

document.querySelector("#sendComment").addEventListener("click", () => {

    const formData = new FormData();
    let srcId = document.querySelector("#srcId").value;
    let comment = document.querySelector("#comment").value;

    formData.append("id", srcId);
    formData.append("content", comment);

    fetch("/api/AddComment", {
        method: "post",
        body: formData
    })
        .then(response => {
            if (response.ok) {
                document.getElementById("comment").value = "";
                loadComments();
            }
            else {
                Swal.fire("Не удалось добавить комментарий");
            }
        })
});

function loadComments() {

    clearComments();
    let srcId = document.querySelector("#srcId").value;

    fetch(`/api/LoadComments?id=${srcId}`)
        .then(response => response.json())
        .then(data => {

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

        });
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
        console.log(date[i])
        newDate += date[i]
    }

    return newDate;
}