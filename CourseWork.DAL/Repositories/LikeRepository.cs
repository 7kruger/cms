using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.DAL.Repositories
{
	public class LikeRepository : ILikeRepository
	{
		private readonly ApplicationDbContext _db;

		public LikeRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<List<Like>> GetAll()
		{
			return await _db.Likes.ToListAsync();
		}

		public async Task Create(Like like)
		{
			await _db.Likes.AddAsync(like);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(Like like)
		{
			_db.Likes.Remove(like);
			await _db.SaveChangesAsync();
		}
	}
}
