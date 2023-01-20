$(document).ready(() => {
	loadComments();
});

$("#comments").on("click", "#deleteComment", function () {
	const id = $(this).parent().parent().attr('id');
	deleted = deleteComment(id);
	if (deleted) {
		$(this).parent().parent().remove();
	}
});

$(document).on("click", "#sendComment", function () {
	addComment();
})

const addComment = () => {
	let comment = $("#comment").val();
	const srcId = $("#srcId").val();

	$.ajax({
		type: "post",
		url: "/api/addcomment",
		data: { content: comment, id: srcId },
	}).done((data) => {
		$("#comment").val("");
		loadComments();
	}).fail((e) => {
		Swal.fire({
			icon: 'error',
			title: 'Oops...',
			text: 'Не удалось добавить комментарий',
		});
	});
}

const loadComments = () => {
	const srcId = $("#srcId").val();

	clearComments();

	$.ajax({
		type: "get",
		url: "/api/loadcomments",
		async: false,
		data: { id: srcId },
	}).done((comments) => {
		let container = $("<div></div>");

		comments.forEach(x => {
			let div = $("<div class='mb-2 list-group-item list-group-item-light' id='" + x.id + "'></div>");

			let deletePartial = "";
			if (x.canUserDeleteComment) {
				deletePartial = "<div id='deleteComment' class='my-cursor'><img src='/images/trash.svg'></div>";
			}

			let header = $("<div class='d-flex justify-content-between mb-3'>"
								+ "<div><strong id='showProfile' class='my-cursor'>" + x.userName + "</strong> " + getDate(x.date) + "</div>"
								+ deletePartial
							+ "</div>");

			let content = $("<p>" + x.content + "</p>");

			div.append(header);
			div.append(content);
			container.append(div);
		});

		$("#comments").append(container);

	}).fail((e) => {
		Swal.fire({
			icon: 'error',
			title: 'Oops...',
			text: 'Не удалось загрузить комментарии',
		});
	});
}

const clearComments = () => {
	$("#comments").empty();
}

const getDate = (d) => {

	let date = new Date(Date.parse(d)).toUTCString();

	let newDate = "";

	for (var i = 0; i < date.length - 7; i++) {
		newDate += date[i]
	}

	return newDate;
}

const deleteComment = (id) => {
	let deleted = false;

	$.ajax({
		type: "post",
		url: "/api/deletecomment",
		async: false,
		data: { id: id },

	}).done(() => {
		deleted = true;
	}).fail(e => {
		Swal.fire({
			icon: 'error',
			title: 'Oops...',
			text: 'Не удалось удалить комментарий',
		});
	});

	return deleted;
}