using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Repositories;
using CourseWork.DAL;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseWork.Tests.DAL.Repositories;

public class ProfileRepositoryTests
{
	private readonly IRepository<Profile> _repository;

	public ProfileRepositoryTests()
	{
		_repository = GetInMemoryProfileRepository();
	}

	[Fact]
	public async Task GetAllProfiles()
	{
		var profiles = await _repository.GetAll().ToListAsync();

		Assert.NotNull(profiles);
	}

	[Fact]
	public async Task CreateProfile()
	{
		var profile = new Profile { Id = 2, UserId = 2, ImgUrl = string.Empty };
		await _repository.Create(profile);

		Assert.True(_repository.GetAll().Any(x => x.Id == profile.Id));
	}

	[Fact]
	public async Task UpdateProfile()
	{
		var profile = await _repository.GetAll().FirstOrDefaultAsync();
		profile.ImgUrl = "imgurl";

		await _repository.Update(profile);

		Assert.Equal(profile.ImgUrl, _repository.GetAll().FirstOrDefault()?.ImgUrl);
	}

	private IRepository<Profile> GetInMemoryProfileRepository()
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		builder.UseInMemoryDatabase("UnitTestsDatabase");
		var dbcontext = new ApplicationDbContext(builder.Options);
		dbcontext.Database.EnsureCreated();
		return new ProfileRepository(dbcontext);
	}
}
