using CourseWork.Domain.Entities;
using CourseWork.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
	public interface ICommentService
	{
		Task<IBaseResponse<List<Comment>>> LoadComments(string id);
		Task<IBaseResponse<bool>> AddComment(string id, string username, string content);
		Task<IBaseResponse<bool>> DeleteComment(string id, string username);
	}
}
