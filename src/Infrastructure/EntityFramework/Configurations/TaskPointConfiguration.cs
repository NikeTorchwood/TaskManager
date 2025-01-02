using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Common.Resources.Constants.DescriptionConstants;
using static Common.Resources.Constants.TitleConstants;

namespace EntityFramework.Configurations;

internal class TaskPointConfiguration : IEntityTypeConfiguration<TaskPoint>
{
    public void Configure(EntityTypeBuilder<TaskPoint> builder)
    {
        builder.ToTable("TaskPoints");

        builder.HasKey(tp => tp.Id);

        builder.OwnsOne(tp => tp.Title, title =>
        {
            title.Property(t => t.Value)
                .HasColumnName("Title")
                .IsRequired()
                .HasMaxLength(TITLE_MAX_LENGTH);
        });

        builder.OwnsOne(tp => tp.Description, description =>
        {
            description.Property(d => d.Value)
                .HasColumnName("Description")
                .IsRequired()
                .HasMaxLength(DESCRIPTION_MAX_LENGTH);
        });

        builder.Property(tp => tp.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(tp => tp.CreatedAt)
            .IsRequired();

        builder.Property(tp => tp.Deadline)
            .IsRequired();

        builder.Property(tp => tp.StartedAt);

        builder.Property(tp => tp.ClosedAt);

        builder.Property(tp => tp.IsDeleted)
            .HasDefaultValue(false);
    }
}