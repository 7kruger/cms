using CourseWork.DAL;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseWork.Tests.DAL.Repositories;

public class UserRepositoryTests
{
	private readonly IRepository<User> _repository;

	public UserRepositoryTests()
	{
		_repository = GetInMemoryUserRepository();
	}

	[Fact]
	public async Task GetAllUsers()
	{
		var users = await _repository.GetAll().ToListAsync();

		Assert.NotNull(users);
	}

	[Fact]
	public async Task CreateUser()
	{
		var user = new User() { Name = "Test", Password = "TestPassword" };
		await _repository.Create(user);

		Assert.True(_repository.GetAll().Any(x => x.Id == user.Id));
	}

	[Fact]
	public async Task UpdateUser()
	{
		var user = await _repository.GetAll().FirstOrDefaultAsync();
		user.Name = "edited";

		await _repository.Update(user);

		Assert.Equal(user.Name, _repository.GetAll().FirstOrDefault()?.Name);
	}

	[Fact]
	public async Task DeleteUser()
	{
		var user = _repository.GetAll().FirstOrDefault();

		await _repository.Delete(user);

		Assert.Null(_repository.GetAll().FirstOrDefault(x => x.Id == user.Id));
	}

	private IRepository<User> GetInMemoryUserRepository()
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		builder.UseInMemoryDatabase("UnitTestsDatabase");
		var dbcontext = new ApplicationDbContext(builder.Options);
		dbcontext.Database.EnsureCreated();
		return new UserRepository(dbcontext);
	}
}
