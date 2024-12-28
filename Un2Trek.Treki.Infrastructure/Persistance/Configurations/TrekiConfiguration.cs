using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Infrastructure.Persistance;

public class TrekiConfiguration : IEntityTypeConfiguration<Treki>
{
    public void Configure(EntityTypeBuilder<Treki> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(at => at.Id)
         .HasConversion(
             id => id.Value,
             value => TrekiId.From(value))
         .ValueGeneratedNever();

        builder.Property(t => t.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(250);

        builder.OwnsOne(t => t.Location, location =>
        {
            location.Property(l => l.Latitude).HasColumnName("Latitude").IsRequired();
            location.Property(l => l.Longitude).HasColumnName("Longitude").IsRequired();
        });

        builder.Property(s => s.CaptureType)
         .HasConversion(
             captureType => captureType.Value,
             value => CaptureType.FromValue(value));

        builder.HasMany(t => t.ActivityTrekiTrekis)
            .WithOne(att => att.Treki)
            .HasForeignKey(att => att.TrekiId);

        builder
         .Property<byte[]>("RowVersion")
         .IsRowVersion();
    }
}

