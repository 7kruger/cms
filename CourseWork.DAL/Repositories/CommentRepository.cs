using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
			return _db.Comments.Include(x => x.UpvotedUsers)
				.Include(u => u.Creator)
				.ThenInclude(x => x.Profile);
		}
		public async Task Create(Comment comment)
		{
			await _db.Comments.AddAsync(comment);
			await _db.SaveChangesAsync();
		}

        public async Task Update(Comment entity)
        {
			_db.Comments.Update(entity);
			await _db.SaveChangesAsync();
        }

        public async Task Delete(Comment comment)
		{
			_db.Comments.Remove(comment);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteRange(IEnumerable<Comment> comments)
		{
			_db.Comments.RemoveRange(comments);
			await _db.SaveChangesAsync();
		}
	}
}
