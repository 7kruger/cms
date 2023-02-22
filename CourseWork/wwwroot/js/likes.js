let liked = false;

$(document).ready(() => {
	loadLikes();
});

$("#like-clickable").on("click", () => {
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
			$("#like").prop("class", "like fa-solid fa-heart pt-2");
			liked = true;
		} else {
			$("#like").prop("class", "like-disabled fa-regular fa-heart pt-2");
			liked = false;
		}
	}).fail((e) => {
		alert("Не удалось загрузить лайки");
		console.log("err message: " + e.responseText);
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
		alert("Не удалось поставить лайк");
		console.log("err message: " + e.responseText);
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
		alert("Не удалось убрать лайк");
		console.log("err message: " + e.responseText);
	});
}