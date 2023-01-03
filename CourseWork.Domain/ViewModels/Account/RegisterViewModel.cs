using System.ComponentModel.DataAnnotations;

namespace CourseWork.Domain.ViewModels.Account
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Не указан логин")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароль введен неверно")]
		public string ConfirmPassword { get; set; }
	}
}
