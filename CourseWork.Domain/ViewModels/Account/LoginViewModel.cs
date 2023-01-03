using System.ComponentModel.DataAnnotations;

namespace CourseWork.Domain.ViewModels.Account
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Не указан логин")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
