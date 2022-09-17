let comments = document.querySelector("#comments");

loadComments();

document.querySelector("#sendComment").addEventListener("click", () => {

    const formData = new FormData();
    let itemId = document.querySelector("#itemId").value;
    let comment = document.querySelector("#itemId").value;

    formData.append("itemId", itemId);
    formData.append("content", comment);

    fetch("/Home/AddItemComment", {
        method: "post",
        body: formData
    })
        .then(response => {
            if (response.ok) {
                document.getElementById("comment").value = "";
                loadComments();
            }
            else {
                alert("Не удалось добавить комментарий");
            }
        })
});

function loadComments() {

    clearComments();
    let itemId = document.querySelector("#itemId").value;

    fetch(`/Home/LoadItemComments?itemId=${itemId}`)
        .then(response => response.json())
        .then(data => {

            data.forEach(index => {

                //мне самому плохо от этого франкенштейна

                let div = document.createElement("div");
                let small = document.createElement("small");
                let strong = document.createElement("strong");
                div.setAttribute("class", "list-group-item list-group-item-light comment");
                let p = document.createElement("p");

                let date = index.date;
                let d = new Date(Date.parse(date));

                strong.textContent = `Автор - "${index.userName}"     ${d.toUTCString()}`;
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