//document.querySelector("#addBtn").addEventListener("click", () => {

//    let collectionId = document.querySelector("#collectionId").value;
//    let itemName = document.querySelector("#addItemName").value;
//    let itemContent = document.querySelector("#addItemContent").value;
//    const formData = new FormData();

//    formData.append("name", itemName);
//    formData.append("content", itemContent);
//    formData.append("collectionId", collectionId);

//    fetch("/Profile/CreateItem", {
//        method: "post",
//        body: formData
//    })
//        .then(response => {
//            if (response.ok) {
//                return response.json();
//            }
//            else {
//                Swal.fire("Не удалось добавить айтем");
//            }
//        })
//        .then(data => {
//            let table = document.querySelector("table");
//            let tr = document.createElement("tr");
//            let tdName = document.createElement("td");
//            let tdAuthor = document.createElement("td");
//            let tdContent = document.createElement("td");
//            let tdDate = document.createElement("td");

//            tdName.textContent = data.name;
//            tdAuthor.textContent = data.author;
//            tdContent.textContent = data.content;
//            tdDate.textContent = data.date;

//            tr.appendChild(tdName);
//            tr.appendChild(tdAuthor);
//            tr.appendChild(tdContent);
//            tr.appendChild(tdDate);

//            table.appendChild(tr);

//            Swal.fire("Айтем был успешно добавлен");
//        })

//});

//function test(object) {
//    console.log(`id: ${object.value}`);
//}

function removeItem(button) {

    let id = button.value;
    console.log(id);
    //let hidden = document.getElementById(id);
    //console.log(hidden);
    //let row = hidden.parentElement.nodeName;
    //console.log(row);
    //row.remove();
}