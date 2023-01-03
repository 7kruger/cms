using CourseWork.Domain.Enum;
using System;

namespace CourseWork.Domain.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
		public bool IsBlocked { get; set; }
		public DateTime RegistrationDate { get; set; }
		public Role Role { get; set; }
		public Profile Profile { get; set; }
	}
}
