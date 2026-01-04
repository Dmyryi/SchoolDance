using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class SheduleConfiguration : IEntityTypeConfiguration<Shedule>
    {
        public void Configure(EntityTypeBuilder<Shedule> builder)
        {
            builder.HasKey(a => a.SheduleId);
            


            

            builder
               .HasOne(s => s.Trainer)
               .WithMany(t => t.Shedules)
               .HasForeignKey(s => s.TrainerId);

            builder
              .HasOne(s => s.DanceType)
              .WithMany(d => d.Shedules)
              .HasForeignKey(s => s.DanceId);

            builder
              .HasMany(s => s.Visits)
              .WithOne(v => v.Shedule)
              .HasForeignKey(v => v.SheduleId);
              

        }
    }
}
