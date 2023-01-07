using System.Collections.Generic;

namespace CourseWork.Domain.Entities
{
	public class Tag
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public List<Collection> Collections { get; set; } = new List<Collection>();
		public List<Item> Items { get; set; } = new List<Item>();
	}
}
