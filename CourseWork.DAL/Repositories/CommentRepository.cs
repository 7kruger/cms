using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.DAL.Repositories
{
	public class CommentRepository : ICommentRepository
	{
		private readonly ApplicationDbContext _db;

		public CommentRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public IQueryable<Comment> GetAll()
		{
			return _db.Comments;
		}
		public async Task Create(Comment comment)
		{
			await _db.Comments.AddAsync(comment);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Comment comment)
		{
			_db.Comments.Remove(comment);
			await _db.SaveChangesAsync();
		}
	}
}
