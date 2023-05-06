using Microsoft.AspNetCore.Mvc;

namespace CourseWork.ViewModels.Item;

public class ItemViewModel
{
	[HiddenInput(DisplayValue = false)]
	public string Id { get; set; }
	[HiddenInput(DisplayValue = false)]
	public string CollectionId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Author { get; set; }
	public DateTime Date { get; set; }
	public string ImgUrl { get; set; }
	public string CollectionName { get; set; }
	public long LikesCount { get; set; }
	public long CommentsCount { get; set; }
	public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
}
