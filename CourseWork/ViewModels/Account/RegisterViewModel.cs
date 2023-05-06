using System.ComponentModel.DataAnnotations;

namespace CourseWork.ViewModels.Account;

public class RegisterViewModel
{
	[Required(ErrorMessage = "Не указан логин")]
	public string Name { get; set; }

	[Required(ErrorMessage = "Не указан пароль")]
	[MinLength(3, ErrorMessage = "Пароль должен содержать минимум 3 символа")]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	[DataType(DataType.Password)]
	[Compare("Password", ErrorMessage = "Пароли не совпадают")]
	public string ConfirmPassword { get; set; }
}
