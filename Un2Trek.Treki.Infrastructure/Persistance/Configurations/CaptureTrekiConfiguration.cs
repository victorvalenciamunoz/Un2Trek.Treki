using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Infrastructure.Persistance;

internal class CaptureTrekiConfiguration : IEntityTypeConfiguration<UserTrekiCapture>
{
    public void Configure(EntityTypeBuilder<UserTrekiCapture> builder)
    {
        builder.HasKey(utc => new { utc.UserId, utc.TrekiId, utc.ActivityId });

        builder.Property(utc => utc.ActivityId)
            .HasConversion(
                id => id.Value,
                value => ActivityId.From(value));

        builder.Property(utc => utc.TrekiId)
            .HasConversion(
                id => id.Value,
                value => TrekiId.From(value));

        builder
            .HasOne(utc => utc.User)
            .WithMany(u => u.UserTrekiCaptures)
            .HasForeignKey(utc => utc.UserId);

        builder
            .HasOne(utc => utc.Treki)
            .WithMany(t => t.UserTrekiCaptures)
            .HasForeignKey(utc => utc.TrekiId);

        builder
            .HasOne(utc => utc.ActivityTreki)
            .WithMany(at => at.UserTrekiCaptures)
            .HasForeignKey(utc => utc.ActivityId);
    }
}
