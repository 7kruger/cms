using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Repositories;
using CourseWork.DAL;
using Microsoft.EntityFrameworkCore;
using CourseWork.DAL.Entities;
using Xunit;

namespace CourseWork.Tests.DAL.Repositories;

public class LikeRepositoryTests
{
	private readonly ILikeRepository _repository;

	public LikeRepositoryTests()
	{
		_repository = GetInMemoryLikeRepository();
		FillDatabase();
	}

	[Fact]
	public async Task GetAllLikes()
	{
		var likes = await _repository.GetAll().ToListAsync();

		Assert.NotNull(likes);
		Assert.True(likes.Any());
	}

	[Fact]
	public async Task CreateLike()
	{
		var like = new Like { SrcId = Guid.NewGuid().ToString(), UserName = "New Test User 3" };

		await _repository.Create(like);

		Assert.NotNull(_repository.GetAll().FirstOrDefault(x => x.Id == like.Id));
	}

	[Fact]
	public async Task DeleteLike()
	{
		var like = await _repository.GetAll().FirstOrDefaultAsync();

		await _repository.Delete(like);

		Assert.Null(_repository.GetAll().FirstOrDefault(x => x.Id == like.Id));
	}

	private ILikeRepository GetInMemoryLikeRepository()
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		builder.UseInMemoryDatabase("UnitTestsDatabase");
		var dbcontext = new ApplicationDbContext(builder.Options);
		dbcontext.Database.EnsureCreated();
		return new LikeRepository(dbcontext);
	}

	private void FillDatabase()
	{
		var likes = new List<Like>()
		{
			new Like { SrcId = Guid.NewGuid().ToString(), UserName = "Test User" },
			new Like { SrcId = Guid.NewGuid().ToString(), UserName = "Test User 1" },
			new Like { SrcId = Guid.NewGuid().ToString(), UserName = "Test User 2" },
		};

		likes.ForEach(x => _repository.Create(x));
	}
}
