using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Infrastructure.Persistance;

public class ActivityTrekiTrekiConfiguration : IEntityTypeConfiguration<ActivityTrekiTreki>
{
    public void Configure(EntityTypeBuilder<ActivityTrekiTreki> builder)
    {
        builder.ToTable("ActivityTrekiTreki");
        builder.HasKey(att => new { att.ActivityTrekiId, att.TrekiId });

        builder
            .HasOne(att => att.ActivityTreki)
            .WithMany(at => at.ActivityTrekiTrekis)
            .HasForeignKey(att => att.ActivityTrekiId);

        builder
            .HasOne(att => att.Treki)
            .WithMany(t => t.ActivityTrekiTrekis)
            .HasForeignKey(att => att.TrekiId);
    }
}
