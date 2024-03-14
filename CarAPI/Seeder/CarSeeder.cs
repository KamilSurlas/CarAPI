using CarAPI.Entities;
using CarAPI.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Seeder
{
    public class CarSeeder : ICarSeeder
    {
        private readonly CarDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public CarSeeder(CarDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public void Seed()
        {
            if (_context.Database.CanConnect())
            {
                if (!_context.Roles.Any())
                {
                    var roles = GetRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }
                if (!_context.Users.Where(u => u.RoleId == 2).Any())
                {
                    var admin = CreateAdminAccount();
                    _context.Users.Add(admin);
                    _context.SaveChanges();
                }
                if (!_context.Users.Where(u => u.RoleId == 3).Any())
                {
                    var mechanic = CreateMechanicAccount();
                    _context.Users.Add(mechanic);
                    _context.SaveChanges();
                }
                if (!_context.Users.Where(u => u.RoleId == 4).Any())
                {
                    var insurer = CreateInsurerAccount();
                    _context.Users.Add(insurer);
                    _context.SaveChanges();
                }
                if (!_context.Cars.Any())
                {
                    var cars = GetCars();
                    _context.Cars.AddRange(cars);
                    _context.SaveChanges();
                }
            }
        }

        private User CreateInsurerAccount()
        {
            var insurer = new User()
            {
                DateOfBirth = new DateTime(2000, 01, 01),
                FirstName = "Insurer",
                LastName = "Insurer",
                Email = "insurer@wp.pl",
                HashedPassword = null!,
                RoleId = 4
            };

            insurer.HashedPassword = _passwordHasher.HashPassword(insurer, "insurer123@!");
            return insurer;
        }

        private User CreateMechanicAccount()
        {
            var mechanic = new User()
            {
                DateOfBirth = new DateTime(2000, 01, 01),
                FirstName = "Mechanic",
                LastName = "Mechanic",
                Email = "mechanic@wp.pl",
                HashedPassword = null!,
                RoleId = 3
            };

            mechanic.HashedPassword = _passwordHasher.HashPassword(mechanic, "mechanic123@!");
            return mechanic;
        }

        private User CreateAdminAccount()
        {
            var admin = new User()
            {
                DateOfBirth = new DateTime(2000, 01, 01),
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@wp.pl",
                HashedPassword = null!,
                RoleId = 2
            };

            admin.HashedPassword = _passwordHasher.HashPassword(admin, "admin123@!");
            return admin;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
         {
             new Role()
             {
                 RoleName = "User"
             },
             new Role()
             {
                 RoleName = "Admin"
             },
             new Role()
             {
                 RoleName = "Mechanic"
             },
             new Role()
             {
                 RoleName = "Insurer"
             }
         };
            return roles;
        }
        private IEnumerable<Car> GetCars()
        {
            var cars = new List<Car>()
            {
                new Car()
                {
                    BrandName = "Toyota",
                    ModelName = "Yaris",
                    ProductionYear = 2019,
                    Mileage = 10.0D,
                    RegistrationNumber = "GD QWE12",
                    Drivetrain = Drivetrain.FWD,
                    BodyType = BodyType.Hatchback,
                    Engine = new Engine()
                    {
                        Horsepower = 68,
                        Displacement = 1.2M,
                        FuelType = FuelType.Hybrid
                    },
                    OcInsurance = new Insurance()
                    {
                        StartDate = new DateTime(2019, 6, 15),
                        EndDate = new DateTime(2019, 6, 15).AddMonths(12),
                        PolicyNumber = "1234QWERTY",
                    },
                    TechnicalReviews = new List<TechnicalReview>()
                    {
                        new TechnicalReview()
                        {
                            TechnicalReviewDate = new DateTime(2019, 6, 20),
                            Description = "First technical review after production",
                            TechnicalReviewResult = TechnicalReviewResult.Positive
                        }
                    }



                },
                new Car()
                {
                    BrandName = "Opel",
                    ModelName = "Corsa",
                    ProductionYear = 2010,
                    Mileage = 598.15D,
                    RegistrationNumber = "ST 123SD",
                    Drivetrain = Drivetrain.FWD,
                    BodyType = BodyType.Hatchback,
                    Engine = new Engine()
                    {
                        Horsepower = 80,
                        Displacement = 1.4M,
                        FuelType = FuelType.Gasoline
                    },
                    OcInsurance = new Insurance()
                    {
                        StartDate = new DateTime(2010, 2, 1),
                        EndDate = new DateTime(2019, 2, 1).AddMonths(12),
                        PolicyNumber = "4321ASOP1W",
                    },
                    TechnicalReviews = new List<TechnicalReview>()
                    {
                        new TechnicalReview()
                        {
                            TechnicalReviewDate = new DateTime(2010,1, 20),
                            Description = "First technical review after production",
                            TechnicalReviewResult = TechnicalReviewResult.Positive
                        }
                    }


                }
            };
            return cars;
        }
    }
}
