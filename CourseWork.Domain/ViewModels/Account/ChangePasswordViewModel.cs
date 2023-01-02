using System.ComponentModel.DataAnnotations;

namespace CourseWork.Domain.ViewModels.Account
{
	public class ChangePasswordViewModel
	{
		[Required]
		public string Name { get; set; }
		[Required(ErrorMessage = "Введите старый пароль")]
		public string OldPassword { get; set; }
		[Required(ErrorMessage = "Введите новый пароль")]
		public string NewPassword { get; set; }
		[Required(ErrorMessage = "Повторите новый пароль")]
		public string ConfirmPassword { get; set; }
	}
}
