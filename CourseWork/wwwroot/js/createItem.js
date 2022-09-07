document.querySelector("#createBtn").addEventListener("click", () => {

    let name = document.querySelector("#name").value;
    let content = document.querySelector("#content").value;
    let radio = document.querySelector('input[name="collectionId"]:checked');
    let image = document.querySelector("#image").files[0];

    const formData = new FormData();
    formData.append("name", name);
    formData.append("content", content);
    formData.append("image", image);
    if (radio != null) {
        formData.append("collectionId", radio.value);
    }

    fetch("/Profile/CreateItem", {
        method: "post",
        headers: {
            "Accept": "application/json",
        },
        body: formData
    })
        .then(response => {
            if (response.ok) {
                document.querySelector("#name").value = "";
                document.querySelector("#content").value = "";
                document.querySelector("#image").files[0] = null;

                Swal.fire("айтем был успешно добавлен");

                //тернарный оператор не работал -_-
                if (radio.checked === true) {
                    radio.checked = false;
                }
                else {
                    radio.checked = true;
                }
            }
        })
})