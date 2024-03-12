using AutoMapper;
using CarAPI.Authorization;
using CarAPI.Entities;
using CarAPI.Enums;
using CarAPI.Exceptions;
using CarAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace CarAPI.Services
{
    public class CarService : ICarService
    {
        private readonly CarDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        public CarService(CarDbContext context, IMapper mapper, ILogger<CarService> logger, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }



        public CarDto GetById(int id)
        {
            var car = _context
                  .Cars
                  .Include(c => c.CarRepairs)
                .Include(c => c.Engine)
                .Include(c => c.TechnicalReviews)
                .Include(c => c.OcInsurance)
                  .FirstOrDefault(c => c.Id == id);
            if (car is null) throw new ContentNotFoundException($"Car with id: {id} was not found");

            var result = _mapper.Map<CarDto>(car);
            return result;
        }

        public PageResult<CarDto> GetAll(Query query)
        {
            var baseQuery = _context
              .Cars
              .Include(c => c.CarRepairs)
              .Include(c => c.Engine)
              .Include(c => c.TechnicalReviews)
              .Include(c => c.OcInsurance)
              .Where(c => query.SearchPhrase == null || (c.BrandName.ToLower().Contains(query.SearchPhrase.ToLower())
              || c.ModelName.ToLower().Contains(query.SearchPhrase.ToLower())
              || c.RegistrationNumber.ToLower().Contains(query.SearchPhrase.ToLower())));


            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Car, object>>>()
                {
                    {nameof(Car.BrandName), c => c.BrandName },
                    {nameof(Car.ModelName), c => c.ModelName },
                    {nameof(Car.RegistrationNumber), c => c.RegistrationNumber },
                    {nameof(Car.Mileage), c => c.Mileage },
                    {nameof(Car.ProductionYear), c => c.ProductionYear }
                };

                var selected = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selected)
                    : baseQuery.OrderByDescending(selected);
            }


            var cars = baseQuery
              .Skip(query.PageSize * (query.PageNumber - 1))
              .Take(query.PageSize)
              .ToList();
            var carResults = _mapper.Map<List<CarDto>>(cars);

            var result = new PageResult<CarDto>(carResults,baseQuery.Count() , query.PageSize, query.PageNumber);

            return result;
        }

        public int Create(NewCarDto dto)
        {
            if (dto.OcInsuranceEndDate <= dto.OcInsuranceStartDate)
            {
               throw new InvalidInsuranceDateException("Insurance end date can not be earlier or equal than insurance start date");
            }
            var car = _mapper.Map<Car>(dto);
            car.CreatedByUserId = _userContextService.UserId;
            car.OcInsurance.AddedByUserId = _userContextService.UserId;
           
            _context.Add(car);
            _context.SaveChanges();
            _logger.LogInformation($"Car with id: {car.Id} has been created by user with id: {_userContextService.UserId}");
            return car.Id;
        }

        public void Delete(int carId)
        {
            _logger.LogWarning($"Car with id: {carId} delete action invoked by user with id: {_userContextService.UserId}");

            var car = _context.Cars        
                 .FirstOrDefault(c => c.Id == carId);

            if (car is null) throw new ContentNotFoundException($"Car with id: {carId} was not found");
            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, car, new ResourceOperationRequirement(ResourceOperationType.Delete)).Result;
            if (!authResult.Succeeded)
            {
                throw new ForbiddenException("Permission denied");
            }
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

            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, car, new ResourceOperationRequirement(ResourceOperationType.Update)).Result;
            if(!authResult.Succeeded)
            {
                throw new ForbiddenException("Permission denied");
            }
            _mapper.Map(dto, car);

            _context.SaveChanges();
            _logger.LogInformation($"Car with id: {car.Id} has been updated by user with id: {_userContextService.UserId}");
        }
        public CarDto GetByRegistrationNumber(string registrationNumber)
        {
            var car =  _context
                  .Cars
                  .Include(c => c.CarRepairs)
                .Include(c => c.Engine)
                .Include(c => c.TechnicalReviews)
                .Include(c => c.OcInsurance)
                  .FirstOrDefault(c => c.RegistrationNumber == registrationNumber);
            if (car is null) throw new ContentNotFoundException($"Car with registration number: {registrationNumber} was not found");

            var result = _mapper.Map<CarDto>(car);
            return result;
        }
    }
}
