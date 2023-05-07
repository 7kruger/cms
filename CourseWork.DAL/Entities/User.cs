using CourseWork.Domain.Enum;
using System.Collections.Generic;
using System;

namespace CourseWork.DAL.Entities;

public class User
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Password { get; set; }
	public bool IsBlocked { get; set; }
	public DateTime RegistrationDate { get; set; }
	public Role Role { get; set; }
	public Profile Profile { get; set; }
	public List<Comment> UpvotedComments { get; set; }
}
