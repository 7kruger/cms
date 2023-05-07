using System.Collections.Generic;

namespace CourseWork.DAL.Entities;

public class Tag
{
	public long Id { get; set; }
	public string Name { get; set; }
	public List<Collection> Collections { get; set; } = new();
	public List<Item> Items { get; set; } = new();
}
