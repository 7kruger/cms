using System.ComponentModel.DataAnnotations;

namespace CourseWork.ViewModels.Account;

public class LoginViewModel
{
	[Required(ErrorMessage = "Не указан логин")]
	public string Name { get; set; }

	[Required(ErrorMessage = "Не указан пароль")]
	[MinLength(3, ErrorMessage = "Минимальная длина пароля 3 символа")]
	[DataType(DataType.Password)]
	public string Password { get; set; }
}
