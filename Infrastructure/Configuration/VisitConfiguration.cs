using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.HasKey(v => v.VisitId);

            builder
                .HasOne(v => v.Subscription)
                .WithMany(s => s.Visits)
                .HasForeignKey(v => v.SubId);

            builder
                .HasOne(v => v.Shedule)
                .WithMany(s => s.Visits)
                .HasForeignKey(v => v.SheduleId);

            builder.Property(v => v.ActualDate).IsRequired();
        }
    }
}

