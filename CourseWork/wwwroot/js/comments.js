let comments = document.querySelector("#comments");

loadComments();

document.querySelector("#sendComment").addEventListener("click", () => {

    const formData = new FormData();
    let collectionId = document.querySelector("#collectionId").value;
    let comment = document.querySelector("#comment").value;
    console.log(comment);
    console.log(collectionId);

    formData.append("collectionId", collectionId);
    formData.append("content", comment);

    fetch("/Home/AddComment", {
        method: "post",
        body: formData
    })
        .then(response => {
            if (response.ok) {
                loadComments();
            }
            else {
                alert("Не удалось добавить комментарий");
            }
        })

});

function loadComments() {

    clearComments();
    let collectionId = document.querySelector("#collectionId").value;

    fetch(`/Home/LoadComments?collectionId=${collectionId}`)
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