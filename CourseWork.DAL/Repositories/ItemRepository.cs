using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Entities;
using System.Linq;
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

		public IQueryable<Item> GetAll()
		{
			return _db.Items;
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
