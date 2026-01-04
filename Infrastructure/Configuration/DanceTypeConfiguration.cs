using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class DanceTypeConfiguration : IEntityTypeConfiguration<DanceType>
    {
        public void Configure(EntityTypeBuilder<DanceType> builder)
        {
            builder.HasKey(a => a.DanceId);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(255);
            builder.Property(a => a.Category).IsRequired().HasMaxLength(255);

            builder
                .HasMany(d => d.Shedules)
                .WithOne(s => s.DanceType)
                .HasForeignKey(s => s.DanceId);
        }


    }
}
