using System;

namespace CourseWork.Domain.Entities
{
	public class Comment
	{
		public int Id { get; set; }
		public string SrcId { get; set; }
		public string UserName { get; set; }
		public string Content { get; set; }
		public DateTime Date { get; set; }
	}
}
