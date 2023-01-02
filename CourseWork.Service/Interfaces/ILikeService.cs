using CourseWork.Domain.Response;
using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
	public interface ILikeService
	{
		Task<IBaseResponse<string>> LoadLikes(string id, string username); // return json
		Task<IBaseResponse<bool>> AddLike(string id, string username);
		Task<IBaseResponse<bool>> RemoveLike(string id, string username);
	}
}
