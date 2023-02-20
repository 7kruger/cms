const showUserInfoModal = new bootstrap.Modal($("#showUserInfoModal"), { keyboard: false });

$(document).on("click", "#showProfile", function () {
	const name = $(this).text();
	showProfile(name);
});

const showProfile = (name) => {
	$.ajax({
		type: "get",
		url: "/profile/showprofile",
		data: { name: name },
	}).done((data) => {
		$("#username").text(data.username);
		$("#avatar").prop("src", data.imgRef);
		$("#collectionsCount").text("Создано коллекций: " + data.collectionsCount);
		$("#itemsCount").text("Создано айтемов: " + data.itemsCount);
		showUserInfoModal.show();
	}).fail(e => {
		alert("something went wrong")
	});
};