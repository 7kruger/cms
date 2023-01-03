using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.DAL.Repositories
{
	public class UserRepository : IRepository<User>
	{
		private readonly ApplicationDbContext _db;

		public UserRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public IQueryable<User> GetAll()
		{
			return _db.Users;
		}

		public async Task Create(User entity)
		{
			await _db.Users.AddAsync(entity);
			await _db.SaveChangesAsync();
		}

		public async Task Update(User entity)
		{
			_db.Users.Update(entity);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(User entity)
		{
			_db.Users.Remove(entity);
			await _db.SaveChangesAsync();
		}
	}
}
