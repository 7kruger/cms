using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
	public interface ILikeService
	{
		Task<string> LoadLikes(string id, string username); // returns json
		Task<bool> AddLike(string id, string username);
		Task<bool> RemoveLike(string id, string username);
	}
}
