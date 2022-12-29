using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

		public async Task<List<Collection>> GetAll()
		{
			return await _db.Collections.Include(i => i.Items).ToListAsync();
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
