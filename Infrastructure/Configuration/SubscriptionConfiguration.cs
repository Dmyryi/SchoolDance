using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(a => a.SubId);
            builder.Property(s => s.Status).IsRequired().HasMaxLength(50);

            builder
             .HasOne(s => s.Student)
             .WithMany(u => u.Subscriptions)
             .HasForeignKey(u => u.StudentId);


           
            builder
                .HasOne(s => s.Tariff)
                .WithMany(t => t.Subscriptions) 
                .HasForeignKey(s => s.TariffId); 

            // Дисконт к Подпискам
            builder
                .HasOne(s => s.Discount)
                .WithMany(d => d.Subscriptions) 
                .HasForeignKey(s => s.DiscountId)
                .IsRequired(false);

            builder
                .HasMany(s => s.Visits)
                .WithOne(v => v.Subscription)
                .HasForeignKey(v => v.SubId);

        }


    }
}
