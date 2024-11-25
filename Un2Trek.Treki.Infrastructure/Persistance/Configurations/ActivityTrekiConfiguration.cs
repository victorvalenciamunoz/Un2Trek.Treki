using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Infrastructure.Persistance;

public class ActivityTrekiConfiguration : IEntityTypeConfiguration<ActivityTreki>
{
    public void Configure(EntityTypeBuilder<ActivityTreki> builder)
    {
        builder.HasKey(at => at.Id);

        builder.Property(at => at.Id)
            .HasConversion(
                id => id.Value,
                value => ActivityId.From(value))
            .ValueGeneratedNever();

        builder.Property(at => at.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(at => at.Description)
            .HasMaxLength(150);

        builder.Property(at => at.ValidFromDate)
            .IsRequired();

        builder.Property(at => at.ValidToDate);

        builder.HasMany(at => at.ActivityTrekiTrekis)
            .WithOne(att => att.ActivityTreki)
            .HasForeignKey(att => att.ActivityTrekiId);
    }
}
