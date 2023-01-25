using CourseWork.Domain.Models;
using CourseWork.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
	public interface ICommentService
	{
		Task<IBaseResponse<List<CommentModel>>> LoadComments(string id, string username, bool isAdmin);
		Task<IBaseResponse<CommentModel>> AddComment(string id, string username, string content);
		Task<IBaseResponse<bool>> DeleteComment(int id);
	}
}
