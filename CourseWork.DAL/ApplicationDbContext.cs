using CourseWork.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.DAL
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
		{
			Database.EnsureCreated();
		}

		public DbSet<Collection> Collections { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			string adminRoleName = "admin";
			string userRoleName = "user";

			string adminName = "admin";
			string adminPassword = "123";

			Role adminRole = new Role { Id = 1, Name = adminRoleName };
			Role userRole = new Role { Id = 2, Name = userRoleName };
			User adminUser = new User
			{
				Id = 1,
				Name = adminName,
				Password = adminPassword,
				RoleId = adminRole.Id,
				RegistrationDate = System.DateTime.Now,
				Status = true
			};

			modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
			modelBuilder.Entity<User>().HasData(new User[] { adminUser });
			base.OnModelCreating(modelBuilder);
		}
	}
}
