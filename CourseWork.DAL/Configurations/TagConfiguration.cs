using CourseWork.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseWork.DAL.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
	public void Configure(EntityTypeBuilder<Tag> builder)
	{
		builder.ToTable("Tags");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name).IsRequired();

		builder.HasMany(x => x.Collections)
			.WithMany(x => x.Tags);
	}
}
