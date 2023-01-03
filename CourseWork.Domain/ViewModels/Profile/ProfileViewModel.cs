namespace CourseWork.Domain.ViewModels.Profile
{
	public class ProfileViewModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string ImgRef { get; set; }
		public int CollectionsCount { get; set; }
		public int ItemsCount { get; set; }
		public int LikesCount { get; set; }
	}
}
