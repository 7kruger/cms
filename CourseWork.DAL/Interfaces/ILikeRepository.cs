using CourseWork.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.DAL.Interfaces
{
	public interface ILikeRepository
	{
		Task<List<Like>> GetAll();
		Task Create(Like like);
		Task Delete(Like like);
	}
}
