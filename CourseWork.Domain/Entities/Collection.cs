using CourseWork.Domain.Enum;
using System;
using System.Collections.Generic;

namespace CourseWork.Domain.Entities
{
	public class Collection
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		public Theme Theme { get; set; }
		public string ImgRef { get; set; }
		public DateTime Date { get; set; }
		public List<Item> Items { get; set; } = new List<Item>();
		public List<Tag> Tags { get; set; } = new List<Tag>();
	}
}
