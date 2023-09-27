using AutoMapper;
using CarAPI.Entities;
using CarAPI.Exceptions;
using CarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Services
{
    public class RepairService : IRepairService
    {
        private readonly CarDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public RepairService(CarDbContext context, IMapper mapper, ILogger<TechnicalReviewService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        private Car GetCarById(int carId) 
        {
            var car = _context.Cars
                 .Include(c => c.TechnicalReviews)
                 .FirstOrDefault(c => c.Id == carId);

            if (car is null) throw new ContentNotFoundException($"Car with id: {carId} was not found");

            return car;
        }
        public int Create(int carId, NewRepairDto dto)
        {
            var car = GetCarById(carId);

            var repairEntity = _mapper.Map<Repair>(dto);
            repairEntity.CarId = carId;

            _context.Repairs.Add(repairEntity);
            _context.SaveChanges();
            _logger.LogInformation($"Technical review with id: {repairEntity.Id} has been created (car id: {carId})");

            return repairEntity.Id;
        }
    }
}
