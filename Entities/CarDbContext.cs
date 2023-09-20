﻿using CarAPI.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection.Emit;

namespace CarAPI.Entities
{
    public class CarDbContext: DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Insurance> Insurances { get; set;}
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<TechnicalReview> TechnicalReviews { get;set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Engine>().Property(e => e.Displacement).HasColumnType("decimal(3,1)");
            modelBuilder.Entity<Repair>().Property(r => r.RepairCost).HasColumnType("decimal(7,3)");
            modelBuilder.Entity<Car>().Property(c => c.BodyType).HasConversion(new EnumToStringConverter<BodyType>());
            modelBuilder.Entity<Engine>().Property(e => e.FuelType).HasConversion(new EnumToStringConverter<FuelType>());
            modelBuilder.Entity<TechnicalReview>().Property(t => t.TechnicalReviewResult).HasConversion(new EnumToStringConverter<TechnicalReviewResult>());
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CarsDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
           
        }
    }

    
}
