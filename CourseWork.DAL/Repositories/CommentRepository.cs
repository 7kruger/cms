using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

		public async Task<List<Comment>> GetAll()
		{
			return await _db.Comments.ToListAsync();
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
