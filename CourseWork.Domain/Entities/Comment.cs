using System;

namespace CourseWork.Domain.Entities
{
	public class Comment
	{
		public long Id { get; set; }
		public string SrcId { get; set; }
		public string Content { get; set; }
		public DateTime Date { get; set; }
		public User User { get; set; }
	}
}
