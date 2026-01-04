using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class TariffConfiguration : IEntityTypeConfiguration<Tariff>
    {
        public void Configure(EntityTypeBuilder<Tariff> builder)
        {
            builder.HasKey(t => t.TariffId);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(255);
            builder.Property(t => t.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(t => t.DaysValid).IsRequired();

            builder
                .HasMany(t => t.Subscriptions)
                .WithOne(s => s.Tariff)
                .HasForeignKey(s => s.TariffId);
        }
    }
}

