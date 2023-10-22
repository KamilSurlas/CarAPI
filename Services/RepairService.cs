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
                 .Include(c => c.CarRepairs)
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

        public IEnumerable<RepairDto> GetAll(int carId)
        {
            var car = GetCarById(carId);

            var results = _mapper.Map<List<RepairDto>>(car.CarRepairs);
            return results;
        }

        public RepairDto GetById(int carId, int repairId)
        {
            var car = GetCarById(carId);
            var repair = GetRepairById(repairId);
            if(repair.CarId != carId) throw new ContentNotFoundException($"Provided car id is wrong (car id: {carId})");

            var result = _mapper.Map<RepairDto>(repair);
            return result;

        }
        private Repair GetRepairById(int repairId)
        {
            var repair = _context.Repairs.FirstOrDefault(r => r.Id == repairId);
            if (repair is null) throw new ContentNotFoundException($"Repair with id: {repairId} was not found");
            return repair;
        }

        public void DeleteAll(int carId)
        {
            _logger.LogWarning($"Repairs delete action invoked (car id: {carId})");
            var car = GetCarById(carId);
            _context.RemoveRange(car.CarRepairs);
            _context.SaveChanges();
        }

        public void DeleteById(int carId, int repairId)
        {
            _logger.LogWarning($"Repair with id: {repairId} delete action invoked (car id: {carId})");
            var car = GetCarById(carId);
            var repair = GetRepairById(repairId);
            if (repair.CarId != carId) throw new ContentNotFoundException($"Provided car id is wrong (car id: {carId})");

            _context.Repairs.Remove(repair);
            _context.SaveChanges();
        }

        public void UpdateRepair(int carId, int repairId, UpdateRepairDto dto)
        {
            var car = GetCarById(carId);
            var repair = GetRepairById(repairId);
            if (repair.CarId != carId) throw new ContentNotFoundException($"Provided car id is wrong (car id: {carId})");
            _mapper.Map(dto, repair);

            repair.CarId = carId;

            _context.SaveChanges();
            _logger.LogWarning($"Repair with id: {repairId}  has been updated (car id: {carId})");
        }
    }
}
