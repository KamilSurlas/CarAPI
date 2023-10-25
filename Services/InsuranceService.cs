using AutoMapper;
using CarAPI.Entities;
using CarAPI.Exceptions;
using CarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly CarDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public InsuranceService(CarDbContext context, IMapper mapper, ILogger<InsuranceService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        private Car GetCarById(int carId)
        {
            var car = _context.Cars
                 .Include(c => c.OcInsurance)
                 .FirstOrDefault(c => c.Id == carId);

            if (car is null) throw new ContentNotFoundException($"Car with id: {carId} was not found");

            return car;
        }    
        private Insurance GetInsuranceById(int insuranceId)
        {
            var insurance = _context.Insurances.FirstOrDefault(i => i.Id == insuranceId);
            if (insurance is null) throw new ContentNotFoundException($"Insurance with id: {insuranceId} was not found");
            return insurance;
        }
        public InsuranceDto GetInsurance(int carId)
        {
            var car = GetCarById(carId);
            var insurance = GetInsuranceById(car.OcInsurance.Id);
            if (insurance.CarId != carId) throw new ContentNotFoundException($"Provided car id is wrong (car id: {carId})");

            var result = _mapper.Map<InsuranceDto>(insurance);
            return result;
        }

        public void UpdateInsurance(int carId, UpdateInsuranceDto dto)
        {
            var car = GetCarById(carId);
            var insurance = GetInsuranceById(car.OcInsurance.Id);
            if (insurance.CarId != carId) throw new ContentNotFoundException($"Provided car id is wrong (car id: {carId})");
            _mapper.Map(dto, insurance);

            insurance.CarId = carId;

            _context.SaveChanges();
            _logger.LogWarning($"Insurance with id: {insurance.Id}  has been updated (car id: {carId})");
        }
    }
}
