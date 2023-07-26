using CourseWork.DAL;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseWork.Tests.DAL.Repositories;

public class ItemRepositoriesTests
{
	private readonly IRepository<Item> _repository;

	public ItemRepositoriesTests()
	{
		_repository = GetInMemoryItemRepository();
		FillDatabase();
	}

	[Fact]
	public async Task GetAllItems()
	{
		var items = await _repository.GetAll().ToListAsync();

		Assert.NotNull(items);
		Assert.True(items.Any());
		Assert.Equal(3, items.Count);
	}

	[Fact]
	public async Task CreateItem()
	{
		var item = new Item()
		{
			Id = Guid.NewGuid().ToString(),
			Title = "New Title",
			Description = "New Description",
			Author = "New Author"
		};

		await _repository.Create(item);

		Assert.NotNull(_repository.GetAll().FirstOrDefault(x => x.Id == item.Id));
	}

	[Fact]
	public async Task UpdateItem()
	{
		var item = await _repository.GetAll().FirstOrDefaultAsync();
		item.Title = "edited";

		await _repository.Update(item);

		Assert.Equal(item.Title, _repository.GetAll().FirstOrDefault()?.Title);
	}

	[Fact]
	public async Task DeleteItem()
	{
		var item = await _repository.GetAll().FirstOrDefaultAsync();

		await _repository.Delete(item);

		Assert.Null(_repository.GetAll().FirstOrDefault(x => x.Id == item.Id));
	}

	private IRepository<Item> GetInMemoryItemRepository()
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		builder.UseInMemoryDatabase("UnitTestsDatabase");
		var dbcontext = new ApplicationDbContext(builder.Options);
		dbcontext.Database.EnsureCreated();
		return new ItemRepository(dbcontext);
	}

	private void FillDatabase()
	{
		var items = new List<Item>()
		{
			new Item { Id = Guid.NewGuid().ToString(), Title = "Title", Description = "Description", Author = "Author" },
			new Item { Id = Guid.NewGuid().ToString(), Title = "Title1", Description = "Description1", Author = "Author1" },
			new Item { Id = Guid.NewGuid().ToString(), Title = "Title2", Description = "Description2", Author = "Author2" },
		};

		items.ForEach(x => _repository.Create(x));
	}
}
