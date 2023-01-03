using CourseWork.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.DAL.Interfaces
{
	public interface ILikeRepository
	{
		IQueryable<Like> GetAll();
		Task Create(Like like);
		Task Delete(Like like);
	}
}
