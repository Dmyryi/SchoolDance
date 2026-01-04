using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.HasKey(a => a.TrainerId);
           
            builder.Property(d => d.Specialization).IsRequired().HasMaxLength(255);

            // User relationship configured in UserConfiguration
            builder
               .HasMany(s => s.Shedules)
               .WithOne(s => s.Trainer)
               .HasForeignKey(u => u.TrainerId);


        }


    }
}
