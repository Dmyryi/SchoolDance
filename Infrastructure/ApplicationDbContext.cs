using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.User;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<DanceType> DanceTypes { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Shedule> Shedules { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Visit> Visits { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.ApplyConfiguration(new DanceTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountConfiguration());
            modelBuilder.ApplyConfiguration(new SheduleConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
            modelBuilder.ApplyConfiguration(new TariffConfiguration());
            modelBuilder.ApplyConfiguration(new TrainerConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new VisitConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

