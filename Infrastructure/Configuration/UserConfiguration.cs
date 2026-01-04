using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(u => u.UserId);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(255);
            builder.Property(b => b.Phone).IsRequired().HasMaxLength(15);
            builder.Property(c => c.Email).HasMaxLength(100);
            builder
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255); 
            builder
                .Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder
                .HasOne(u => u.Trainer)
                .WithOne(t => t.User)
                .HasForeignKey<Trainer>(t => t.UserId)
                .IsRequired(false);
            builder
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .IsRequired(false);
        }
    }
}
