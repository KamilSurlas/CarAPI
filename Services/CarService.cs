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
        public CarService(CarDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            return car.Id;
        }
    }
}
