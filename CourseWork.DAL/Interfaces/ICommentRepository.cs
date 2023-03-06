using CourseWork.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.DAL.Interfaces
{
	public interface ICommentRepository
	{
		IQueryable<Comment> GetAll();
		Task Create(Comment comment);
		Task Update(Comment comment);
		Task Delete(Comment comment);
		Task DeleteRange(IEnumerable<Comment> comments);
	}
}
