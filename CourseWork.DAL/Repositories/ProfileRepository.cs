using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.DAL.Repositories
{
	public class ProfileRepository : IRepository<Profile>
	{
		private readonly ApplicationDbContext _db;

		public ProfileRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public IQueryable<Profile> GetAll()
		{
			return _db.Profiles.Include(p => p.User);
		}

		public async Task Create(Profile entity)
		{
			throw new System.NotImplementedException();
		}

		public async Task Update(Profile entity)
		{
			throw new System.NotImplementedException();
		}

		public async Task Delete(Profile entity)
		{
			throw new System.NotImplementedException();
		}
	}
}
