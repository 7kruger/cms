using CourseWork.Domain.Models;
using CourseWork.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
	public interface ICommentService
	{
		Task<IBaseResponse<List<CommentModel>>> LoadComments(string srcId, string username, bool isAdmin);
		Task<IBaseResponse<CommentModel>> AddComment(CommentModel model, string username);
		Task<IBaseResponse<bool>> UpdateComment(CommentModel model);
		Task<IBaseResponse<bool>> DeleteComment(long id);
		Task<IBaseResponse<bool>> Upvote(CommentModel model, string username);
	}
}
