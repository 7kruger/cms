using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.DAL.Repositories
{
	public class LikeRepository : ILikeRepository
	{
		private readonly ApplicationDbContext _db;

		public LikeRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public IQueryable<Like> GetAll()
		{
			return _db.Likes;
		}

		public async Task Create(Like like)
		{
			await _db.Likes.AddAsync(like);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Like like)
		{
			_db.Likes.Remove(like);
			await _db.SaveChangesAsync();
		}
	}
}
