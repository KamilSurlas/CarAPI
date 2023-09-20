using AutoMapper;
using CarAPI.Entities;
using CarAPI.Exceptions;
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
            if (car is null) throw new ContentNotFoundException($"Car with id: {id} was not found");

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
            if (dto.OcInsuranceEndDate <= dto.OcInsuranceStartDate)
            {
               throw new InvalidInsuranceDate("Insurance end date can not be earlier or equal than insurance start date");
            }
            var car = _mapper.Map<Car>(dto);
            _context.Add(car);
            _context.SaveChanges();
            _logger.LogInformation($"Car with id: {car.Id} has been created");
            return car.Id;
        }

        public void Delete(int carId)
        {
            _logger.LogWarning($"Car with id: {carId} delete action invoked");

            var car = _context.Cars        
                 .FirstOrDefault(c => c.Id == carId);

            if (car is null) throw new ContentNotFoundException($"Car with id: {carId} was not found");

            _context.Cars.Remove(car);
            _context.SaveChanges();
        }
        public void Update(int carId, UpdateCarDto dto)
        {
            var car = _context
               .Cars
               .Include(c => c.Engine)
               .FirstOrDefault(c => c.Id == carId);
            if (car is null) throw new ContentNotFoundException($"Car with id: {carId} was not found");

            car.Mileage = dto.Mileage;
            car.Engine.Horsepower = dto.EngineHorsepower;
            car.Engine.Displacement = dto.EngineDisplacement;
            car.Engine.FuelType = dto.FuelType;

            _context.SaveChanges();
            _logger.LogInformation($"Car with id: {car.Id} has been updated");
        }
    }
}
