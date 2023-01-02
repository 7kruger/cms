using CourseWork.Domain.Entities;
using CourseWork.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.DAL
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
			: base(options) 
		{
			Database.EnsureCreated();
		}

		public DbSet<Collection> Collections { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var admin = new User
			{
				Id = 1,
				Name = "admin",
				Password = HashPasswordHelper.HashPassword("admin"),
				Role = Domain.Enum.Role.Admin,
				RegistrationDate = System.DateTime.Now,
				IsBlocked = false,
			};

			modelBuilder.Entity<User>().HasData(new User[] { admin });

			base.OnModelCreating(modelBuilder);
		}
	}
}
