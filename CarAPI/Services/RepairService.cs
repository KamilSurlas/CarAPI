using AutoMapper;
using CarAPI.Authorization;
using CarAPI.Entities;
using CarAPI.Enums;
using CarAPI.Exceptions;
using CarAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Services
{
    public class RepairService : IRepairService
    {
        private readonly CarDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        public RepairService(CarDbContext context, IMapper mapper, ILogger<TechnicalReviewService> logger, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
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
            repairEntity.Car = car;
            repairEntity.AddedByUserId = _userContextService.UserId;
            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, repairEntity, new ResourceOperationRequirement(ResourceOperationType.Create)).Result;
            if (!authResult.Succeeded)
            {
                throw new ForbiddenException("Permission denied");
            }

            _context.Repairs.Add(repairEntity);
            _context.SaveChanges();
            _logger.LogInformation($"Repair with id: {repairEntity.Id} has been created (car id: {carId}) by user with id: {_userContextService.UserId}");

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
            _logger.LogWarning($"Repairs delete action invoked (car id: {carId}) by user with id: {_userContextService.UserId}");
            var car = GetCarById(carId);
            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, car, new ResourceOperationRequirement(ResourceOperationType.Delete)).Result;
            if (!authResult.Succeeded)
            {
                throw new ForbiddenException("Permission denied");
            }
            _context.RemoveRange(car.CarRepairs);
            _context.SaveChanges();
            _logger.LogWarning($"Repairs deleted (car id: {carId}) by user with id: {_userContextService.UserId}");
        }

        public void DeleteById(int carId, int repairId)
        {
            _logger.LogWarning($"Repair with id: {repairId} delete action invoked (car id: {carId}) by user with id: {_userContextService.UserId}");
            var car = GetCarById(carId);
            var repair = GetRepairById(repairId);
            if (repair.CarId != carId) throw new ContentNotFoundException($"Provided car id is wrong (car id: {carId})");
            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, repair, new ResourceOperationRequirement(ResourceOperationType.Delete)).Result;
            if (!authResult.Succeeded)
            {
                throw new ForbiddenException("Permission denied");
            }
            _context.Repairs.Remove(repair);
            _context.SaveChanges();
            _logger.LogWarning($"Repair deleted (repair id: {repair.Id}) by user with id: {_userContextService.UserId}");
        }

        public void UpdateRepair(int carId, int repairId, UpdateRepairDto dto)
        {
            var car = GetCarById(carId);
            var repair = GetRepairById(repairId);
            if (repair.CarId != carId) throw new ContentNotFoundException($"Provided car id is wrong (car id: {carId})");
            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, repair, new ResourceOperationRequirement(ResourceOperationType.Update)).Result;
            if (!authResult.Succeeded)
            {
                throw new ForbiddenException("Permission denied");
            }
            _mapper.Map(dto, repair);

            repair.CarId = carId;

            _context.SaveChanges();
            _logger.LogWarning($"Repair with id: {repairId}  has been updated (car id: {carId}) by user with id:  {_userContextService.UserId}");
        }
    }
}
