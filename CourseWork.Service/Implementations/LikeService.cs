using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Models;
using CourseWork.Domain.Response;
using CourseWork.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CourseWork.Service.Implementations
{
	public class LikeService : ILikeService
	{
		private readonly ILikeRepository _likeRepository;

		public LikeService(ILikeRepository likeRepository)
		{
			_likeRepository = likeRepository;
		}

		public async Task<IBaseResponse<string>> LoadLikes(string id, string username)
		{
			try
			{
				var likes = (await _likeRepository.GetAll()).Where(l => l.SrcId == id);
				bool isCurrentUserLiked = false;
				if (likes.Count() > 1)
				{
					isCurrentUserLiked = likes.Where(l => l.UserName == username) == null ? false : true;
				}
				var count = likes.Count();

				var json = JsonSerializer.Serialize(new
				{
					likesCount = count,
					liked = isCurrentUserLiked
				});

				return new BaseResponse<string>
				{
					StatusCode = StatusCode.OK,
					Data = json
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<string>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[LoadLikes] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<bool>> AddLike(string id, string username)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(id))
				{
					return new BaseResponse<bool>
					{
						StatusCode = StatusCode.InternalServerError,
						Description = "id null"
					};
				}

				var like = new Like
				{
					SrcId = id,
					UserName = username
				};

				await _likeRepository.Create(like);

				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.OK,
					Data = true
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[SetLikes] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<bool>> RemoveLike(string id, string username)
		{
			try
			{
				var like = (await _likeRepository.GetAll()).FirstOrDefault(l => l.SrcId == id && l.UserName == username);

				if (like == null)
				{
					return new BaseResponse<bool>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Can not remove like"
					};
				}

				await _likeRepository.Delete(like);

				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.OK,
					Data = true
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[RemoveLikes] : {ex.Message}"
				};
			}
		}
	}
}
