using CourseWork.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseWork.DAL.Configurations;

public class CollectionConfiguration : IEntityTypeConfiguration<Collection>
{
	public void Configure(EntityTypeBuilder<Collection> builder)
	{
		builder.ToTable("Collections");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
		builder.Property(x => x.Author).IsRequired();
		builder.Property(x =>x.Description).IsRequired();
		builder.Property(x => x.Theme).IsRequired();
		builder.Property(x => x.Date).IsRequired();

		builder.HasMany(x => x.Items)
			.WithOne(x => x.Collection);

		builder.HasMany(x => x.Tags)
			.WithMany(x => x.Collections);
	}
}
