const imgLike = document.querySelector("#like");
let setLikeBtn = document.querySelector("#setLike");
let removeLikeBtn = document.querySelector("#removeLike");
let srcId = document.querySelector("#srcId").value;
let liked = false;

const likeImageurl = "/images/like.png";
const defaultImageurl = "/images/nolike.png";

loadInfo();

function loadInfo() {

    fetch(`/api/loadlikes?id=${srcId}`)
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            else {
                Swal.fire("Не удалось загрузить лайки");
            }
        })
        .then(data => {
            let likesCount = document.querySelector("#likesCount");
            likesCount.textContent = data.likesCount;

            if (data.liked) {
                liked = true;
                imgLike.src = likeImageurl;
            }
            else {
                liked = false;
                imgLike.src = defaultImageurl;
            }
        })
}

function addLike() {

    const formData = new FormData();
    formData.append("id", srcId);

    console.log(srcId)

    fetch("/api/addlike", {
        method: "post",
        body: formData
    })
        .then(response => {
            if (response.ok) {
                loadInfo();
            }
        })
}
function removeLike() {

    const formData = new FormData();
    formData.append("id", srcId);

    fetch("/api/RemoveLike", {
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

    if (liked) {
        removeLike();
    }
    else {
        addLike();
    }
}
