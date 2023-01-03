using CourseWork.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.DAL.Interfaces
{
	public interface ICommentRepository
	{
		Task<List<Comment>> GetAll();
		Task Create(Comment comment);
		Task Delete(Comment comment);
	}
}
