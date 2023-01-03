using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.DAL.Interfaces
{
	public interface IRepository<T>
	{
		Task<List<T>> GetAll();
		Task Create(T entity);
		Task Update(T entity);
		Task Delete(T entity);
	}
}
