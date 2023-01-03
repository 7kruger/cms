using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Repositories;
using CourseWork.Domain.Entities;
using CourseWork.Service.Implementations;
using CourseWork.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CourseWork
{
	public static class Initializer
	{
		public static void InitializeRepositories(this IServiceCollection services)
		{
			services.AddScoped<IRepository<Collection>, CollectionRepository>();
			services.AddScoped<IRepository<Item>, ItemRepository>();
			services.AddScoped<ILikeRepository, LikeRepository>();
			services.AddScoped<ICommentRepository, CommentRepository>();
			services.AddScoped<IRepository<User>, UserRepository>();
			services.AddScoped<IRepository<Profile>, ProfileRepository>();
		}

		public static void InitializeServices(this IServiceCollection services)
		{
			services.AddScoped<ICollectionService, CollectionService>();
			services.AddScoped<IItemService, ItemService>();
			services.AddScoped<ILikeService, LikeService>();
			services.AddScoped<ICommentService, CommentService>();
			services.AddScoped<ICloudStorageService, DropboxService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IProfileService, ProfileService>();
		}
	}
}
