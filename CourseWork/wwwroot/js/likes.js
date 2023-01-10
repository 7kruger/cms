const imgLike = document.querySelector("#like");
let setLikeBtn = document.querySelector("#setLike");
let removeLikeBtn = document.querySelector("#removeLike");
let srcId = document.querySelector("#srcId").value;
let liked = false;

const likeImageurl = "/images/heart-fill.svg";
const defaultImageurl = "/images/heart.svg";

loadInfo();

async function loadInfo() {

    try {
        const response = await fetch(`/api/loadlikes?id=${srcId}`, {
            method: "get",
            headers: {
                "content-type": "application/json"
            }
        })

        if (response.ok) {

            const data = await response.json();

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
        } else {
            Swal.fire({
                html: "<h1>Не удалось загрузить лайки</h1>"
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

async function addLike() {

    const formData = new FormData();
    formData.append("id", srcId);

    try {
        const response = await fetch("/api/addlike", {
            method: "post",
            body: formData
        });

        if (response.ok) {
            loadInfo();
        }
    } catch (e) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
        })
    }
    
}

async function removeLike() {

    const formData = new FormData();
    formData.append("id", srcId);

    try {
        const response = await fetch("/api/removelike", {
            method: "post",
            body: formData
        });

        if (response.ok) {
            loadInfo();
        }
    } catch (e) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
        })
    }    
}

document.querySelector("#imgA").onclick = () => {

    if (liked) {
        removeLike();
    }
    else {
        addLike();
    }
}
