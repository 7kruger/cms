using System;
using CourseWork.Domain.Enum;

namespace CourseWork.Service.Models;

public class UserModel
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Password { get; set; }
	public Role Role { get; set; }
	public bool IsBlocked { get; set; }
	public DateTime RegistrationDate { get; set; }
}
