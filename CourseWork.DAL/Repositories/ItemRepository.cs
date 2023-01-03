using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.DAL.Repositories
{
	public class ItemRepository : IRepository<Item>
	{
		private readonly ApplicationDbContext _db;

		public ItemRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<List<Item>> GetAll()
		{
			return await _db.Items.ToListAsync();
		}

		public async Task Create(Item entity)
		{
			await _db.Items.AddAsync(entity);
			await _db.SaveChangesAsync();
		}

		public async Task Update(Item entity)
		{
			_db.Items.Update(entity);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Item entity)
		{
			_db.Items.Remove(entity);
			await _db.SaveChangesAsync();
		}
	}
}
