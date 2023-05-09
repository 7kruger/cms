using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Service.Implementations
{
	public class LikeService : ILikeService
	{
		private readonly ILikeRepository _likeRepository;

		public LikeService(ILikeRepository likeRepository)
		{
			_likeRepository = likeRepository;
		}

		public async Task<string> LoadLikes(string id, string username)
		{
			try
			{
				var likes = await _likeRepository.GetAll()
					.Where(l => l.SrcId == id)
					.ToListAsync();

				bool isCurrentUserLiked = likes.Any(l => l.UserName == username);

				var count = likes.Count();

				var json = JsonSerializer.Serialize(new
				{
					likesCount = count,
					liked = isCurrentUserLiked
				});

				return json;
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		public async Task<bool> AddLike(string id, string username)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(id))
				{
					return false;
				}

				var like = new Like
				{
					SrcId = id,
					UserName = username
				};

				await _likeRepository.Create(like);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> RemoveLike(string id, string username)
		{
			try
			{
				var like = await _likeRepository.GetAll()
					.FirstOrDefaultAsync(l => l.SrcId == id && l.UserName == username);

				if (like == null)
				{
					return false;
				}

				await _likeRepository.Delete(like);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
