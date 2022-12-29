using CourseWork.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

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
		public virtual List<Item> Items { get; set; }
		public Collection()
		{
			Items = new List<Item>();
		}
	}
}
