using System;
using CourseWork.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseWork.DAL.Configurations
{
	public class ItemConfiguration : IEntityTypeConfiguration<Item>
	{
		public void Configure(EntityTypeBuilder<Item> builder)
		{
			builder.ToTable("Collections");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Title).IsRequired();
			builder.Property(x => x.Description).IsRequired();
			builder.Property(x => x.Author).IsRequired();
			builder.Property(x => x.Date).IsRequired();

			builder.HasOne(x => x.Collection)
				.WithMany(x => x.Items);
		}
	}
}
