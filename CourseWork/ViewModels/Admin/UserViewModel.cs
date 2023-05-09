using CourseWork.Domain.Enum;

namespace CourseWork.ViewModels.Admin;

public class UserViewModel
{
	public int Id { get; set; }
	public string Name { get; set; }
	public Role Role { get; set; }
	public bool IsBlocked { get; set; }
	public DateTime RegistrationDate { get; set; }
}
