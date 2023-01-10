using System;

namespace CourseWork.Domain.Models
{
	public class CommentModel
	{
		public long Id { get; set; }
		public string UserName { get; set; }
		public string Content { get; set; }
		public DateTime Date { get; set; }
		public bool CanUserDeleteComment { get; set; }
	}
}
