
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
		Swal.fire({
			html: `<div style="object-fit: cover; object-position: center; margin: 15px;">`
						+ `<img style="height: 150px; width: 150px; object-fit: cover; object-position: center; border-radius: 100px; box-shadow: 0 0 17px #666;"`
								+`src="${data.imgRef}" onError="/images/person.svg" alt="avatar">`
				+ `</div>`
				+ `<p>Имя: ${data.username}</p>`
				+ `<p>Дата регистрации: ${getDate(data.registrationDate)}</p>`
				+ `<p>Создано коллекций: ${data.collectionsCount}</p>`
				+ `<p>Создано айтемов: ${data.itemsCount}</p>`
				+ `<p>Получено лайков: ${data.likesCount}</p>`
		});
	}).fail(e => {
		Swal.fire({
			icon: 'error',
			title: 'Oops...',
			text: 'Не удалось открыть профиль пользователя',
		});
	});
}