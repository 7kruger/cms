using System.Collections.Generic;
using System.Threading.Tasks;
using CourseWork.Domain.Models;

namespace CourseWork.Service.Interfaces
{
	public interface ICommentService
	{
		Task<List<CommentModel>> LoadComments(string srcId, string username, bool isAdmin);
		Task<CommentModel> AddComment(CommentModel model, string username);
		Task<bool> UpdateComment(CommentModel model);
		Task<bool> DeleteComment(long id);
		Task<bool> Upvote(CommentModel model, string username);
	}
}
