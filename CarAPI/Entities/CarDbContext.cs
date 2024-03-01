using CarAPI.Enums;
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
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public CarDbContext(DbContextOptions<CarDbContext> options)
            :base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Engine>().Property(e => e.Displacement).HasColumnType("decimal(3,1)");
            modelBuilder.Entity<Repair>().Property(r => r.RepairCost).HasColumnType("decimal(7,3)");
            modelBuilder.Entity<Car>().Property(c => c.BodyType).HasConversion(new EnumToStringConverter<BodyType>());       
            modelBuilder.Entity<Car>().Property(c => c.Drivetrain).HasConversion(new EnumToStringConverter<Drivetrain>());
            modelBuilder.Entity<Engine>().Property(e => e.FuelType).HasConversion(new EnumToStringConverter<FuelType>());
            modelBuilder.Entity<TechnicalReview>().Property(t => t.TechnicalReviewResult).HasConversion(new EnumToStringConverter<TechnicalReviewResult>());
            modelBuilder.Entity<Car>().HasIndex(c => c.RegistrationNumber).IsUnique();
            modelBuilder.Entity<Insurance>().HasIndex(i => i.PolicyNumber).IsUnique();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CarsDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
           
           
        }
    }

    
}
