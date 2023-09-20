using AutoMapper;
using CarAPI.Entities;
using CarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Services
{
    public class CarService : ICarService
    {
        private readonly CarDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public CarService(CarDbContext context, IMapper mapper, ILogger<CarService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }



        public CarDto GetById(int id)
        {
            var car = _context
                  .Cars
                  .Include(c => c.CarRepairs)
                .Include(c => c.Engine)
                .Include(c => c.TechnicalReviews)
                  .FirstOrDefault(c => c.Id == id);
            if (car is null) return null;

            var result = _mapper.Map<CarDto>(car);
            return result;
        }

        public IEnumerable<CarDto> GetAll()
        {
            var cars = _context
              .Cars
              .Include(c => c.CarRepairs)
              .Include(c => c.Engine)
              .Include(c => c.TechnicalReviews)
              .ToList();
            var results = _mapper.Map<List<CarDto>>(cars);
            return results;
        }

        public int Create(NewCarDto dto)
        {
            var car = _mapper.Map<Car>(dto);
            _context.Add(car);
            _context.SaveChanges();
            _logger.LogInformation($"Car with id: {car.Id} has been created");
            return car.Id;
        }

        public bool Delete(int carId)
        {
            _logger.LogWarning($"Car with id: {carId} delete action invoked");

            var car = _context.Cars        
                 .FirstOrDefault(c => c.Id == carId);

            if (car is null) return false;

            _context.Cars.Remove(car);
            _context.SaveChanges();
            return true;
        }
        public bool Update(int carId, UpdateCarDto dto)
        {
            var car = _context
               .Cars
               .Include(c => c.Engine)
               .FirstOrDefault(c => c.Id == carId);
            if (car is null) return false;

            car.Mileage = dto.Mileage;
            car.Engine.Horsepower = dto.EngineHorsepower;
            car.Engine.Displacement = dto.EngineDisplacement;
            car.Engine.FuelType = dto.FuelType;

            _context.SaveChanges();
            return true;
        }
    }
}
