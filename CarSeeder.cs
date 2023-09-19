using CarAPI.Entities;
using CarAPI.Enums;

namespace CarAPI
{
    public class CarSeeder   // Class to insert few cars if database is empty
    {
        private readonly CarDbContext _context;
        public CarSeeder(CarDbContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if (_context.Database.CanConnect())
            {
                if (!_context.Cars.Any())
                {
                    var cars = GetCars();
                    _context.Cars.AddRange(cars);
                    _context.SaveChanges();
                }
            }
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
                    BodyType = BodyType.Hatchback,
                    Engine = new Engine()
                    {
                        Horsepower = 68,
                        Displacement = 1.0M,
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
                    BodyType = BodyType.Hatchback,
                    Engine = new Engine()
                    {
                        Horsepower = 80,
                        Displacement = 1.2M,
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
