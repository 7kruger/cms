using CourseWork.Domain.Enum;
using System;

namespace CourseWork.Service.Models;

public class CollectionModel
{
	public int Id { get; set; }
	public string Title { get; set; }
	public Theme Theme { get; set; }
	public string Description { get; set; }
	public string Author { get; set; }
	public DateTime Date { get; set; }
	public string ImgUrl { get; set; }
}
