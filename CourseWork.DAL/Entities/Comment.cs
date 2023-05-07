using System.Collections.Generic;
using System;

namespace CourseWork.DAL.Entities;

public class Comment
{
	public long Id { get; set; }
	public string SrcId { get; set; }
	public long? Parent { get; set; }
	public DateTime Created { get; set; }
	public DateTime Modified { get; set; }
	public string Content { get; set; }
	public int UpvoteCount { get; set; }
	public User Creator { get; set; }
	public List<User> UpvotedUsers { get; set; } = new();
}
