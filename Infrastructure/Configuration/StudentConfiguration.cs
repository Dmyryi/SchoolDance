using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(a => a.StudentId);
            

            // User relationship configured in UserConfiguration
            builder
                .HasMany(s => s.Subscriptions)
                .WithOne(s => s.Student)
                .HasForeignKey(u => u.StudentId);

            
        }
    }
}
