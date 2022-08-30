const imgLike = document.querySelector("#like");
let setLikeBtn = document.querySelector("#setLike");
let removeLikeBtn = document.querySelector("#removeLike");
let collectionId = document.querySelector("#collectionId").value;

const customImageurl = "/images/like.png";
const defaultImageurl = "/images/unlike.png";

loadInfo();

function loadInfo() {

    fetch(`/Home/LoadLikesInfo?collectionId=${collectionId}`)
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            else {
                console.log(response);
                console.log(response.json());
            }
        })
        .then(data => {
            let likesCount = document.querySelector("#likesCount");
            likesCount.textContent = data.likesCount;
            console.log(data.likesCount);

            if (data.liked) {
                imgLike.src = customImageurl;
                setLikeBtn.disabled = true;
                removeLikeBtn.disabled = false;
            }
            else {
                imgLike.src = defaultImageurl;
                setLikeBtn.disabled = false;
                removeLikeBtn.disabled = true;
            }
        })

}

setLikeBtn.onclick = () => {

    const formData = new FormData();
    formData.append("collectionId", collectionId);

    fetch("/Home/SetLike", {
        method: "post",
        body: formData
    })
        .then(response => {
            if (response.ok) {
                loadInfo();
            }
        })

}
removeLikeBtn.onclick = () => {

    const formData = new FormData();
    formData.append("collectionId", collectionId);

    fetch("/Home/RemoveLike", {
        method: "post",
        body: formData
    })
        .then(response => {
            if (response.ok) {
                loadInfo();
            }
        })

}


document.querySelector("#imgA").onclick = () => {



}
