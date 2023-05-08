using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.DAL.Repositories
{
	public class CollectionRepository : IRepository<Collection>
	{
		private readonly ApplicationDbContext _db;

		public CollectionRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public IQueryable<Collection> GetAll()
		{
			return _db.Collections
				.Include(i => i.Items)
				.Include(t => t.Tags);
		}

		public async Task Create(Collection entity)
		{
			await _db.Collections.AddAsync(entity);
			await _db.SaveChangesAsync();
		}

		public async Task Update(Collection entity)
		{
			_db.Collections.Update(entity);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Collection entity)
		{
			_db.Collections.Remove(entity);
			await _db.SaveChangesAsync();
		}
	}
}
