using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.DAL.Repositories
{
	public class TagRepository : IRepository<Tag>
	{
		private readonly ApplicationDbContext _db;

		public TagRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public IQueryable<Tag> GetAll()
		{
			return _db.Tags;
		}

		public async Task Create(Tag entity)
		{
			await _db.Tags.AddAsync(entity);
			await _db.SaveChangesAsync();
		}

		public async Task Update(Tag entity)
		{
			_db.Tags.Update(entity);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Tag entity)
		{
			_db.Tags.Remove(entity);
			await _db.SaveChangesAsync();
		}
	}
}
