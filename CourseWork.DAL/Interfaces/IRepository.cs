using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.DAL.Interfaces
{
	public interface IRepository<T>
	{
		IQueryable<T> GetAll();
		Task Create(T entity);
		Task Update(T entity);
		Task Delete(T entity);
	}
}
