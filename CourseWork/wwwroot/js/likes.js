let liked = false;

$(document).ready(() => {
	loadLikes();
});

$("#like").on("click", () => {
	if (liked) {
		removeLike();
	} else {
		setLike();
	}
});

const loadLikes = () => {
	const srcId = $("#srcId").val();

	$.ajax({
		type: "get",
		url: "/api/loadlikes",
		async: false,
		data: { id: srcId }
	}).done((data) => {
		const obj = JSON.parse(data)
		$("#likesCount").html(obj.likesCount);
		if (obj.liked) {
			$("#like").children().attr("src", "/images/heart-fill.svg");
			liked = true;
		} else {
			$("#like").children().attr("src", "/images/heart.svg");
			liked = false;
		}
	}).fail((e) => {
		Swal.fire({
			html: "<h1>Не удалось загрузить лайки</h1>"
		});
	});
}

const setLike = () => {
	const srcId = $("#srcId").val();

	$.ajax({
		type: "post",
		url: "/api/addlike",
		async: false,
		data: { id: srcId }
	}).done((data) => {
		loadLikes();
	}).fail((e) => {
		Swal.fire({
			html: "<h1>Не удалось поставить лайк</h1>"
		});
	});
}

const removeLike = () => {
	const srcId = $("#srcId").val();

	$.ajax({
		type: "post",
		url: "/api/removelike",
		async: false,
		data: { id: srcId }
	}).done((data) => {
		loadLikes();
	}).fail((e) => {
		Swal.fire({
			html: "<h1>Не удалось убрать лайк</h1>"
		});
	});
}