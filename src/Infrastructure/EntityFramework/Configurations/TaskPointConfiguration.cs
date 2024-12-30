using Domain.Entities;
using Domain.ValueObjects;
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

        builder.Property(tp => tp.Title)
            .HasConversion(
                title => title.Value,
                value => new Title(value))
            .HasColumnName("Title")
            .IsRequired()
            .HasMaxLength(TITLE_MAX_LENGTH);

        builder.Property(tp => tp.Description)
            .HasConversion(
                description => description.Value,
                value => new Description(value))
            .HasColumnName("Description")
            .IsRequired()
            .HasMaxLength(DESCRIPTION_MAX_LENGTH);

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