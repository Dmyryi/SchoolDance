using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(d => d.DiscountId);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(255);
            builder.Property(d => d.Percent).IsRequired();

            builder
                .HasMany(d => d.Subscriptions)
                .WithOne(s => s.Discount)
                .HasForeignKey(s => s.DiscountId)
                .IsRequired(false);
        }
    }
}

