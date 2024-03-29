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
    public class TechnicalReviewService : ITechnicalReviewService
    {
        private readonly CarDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        public TechnicalReviewService(CarDbContext context, IMapper mapper, ILogger<TechnicalReviewService> logger, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }
        public int Create(int carId, NewTechnicalReviewDto dto)
        {
            var car = GetCarById(carId);         

            var technicalReviewEntity = _mapper.Map<TechnicalReview>(dto);
            technicalReviewEntity.CarId = carId;
            technicalReviewEntity.Car = car;
            technicalReviewEntity.AddedByUserId = _userContextService.UserId;
            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, technicalReviewEntity, new ResourceOperationRequirement(ResourceOperationType.Create)).Result;
            if (!authResult.Succeeded)
            {
                throw new ForbiddenException("Permission denied");
            }
            technicalReviewEntity.CarId = carId;

            _context.TechnicalReviews.Add(technicalReviewEntity);
            _context.SaveChanges();
            _logger.LogInformation($"Technical review with id: {technicalReviewEntity.Id} has been created (car id: {carId}) by user with an Id: {_userContextService.UserId}");

            return technicalReviewEntity.Id;
        }

        public TechnicalReviewDto GetById(int carId, int technicalReviewId)
        {
            var car = GetCarById(carId);

            var technicalReview = GetTechnicalReviewById(technicalReviewId);
            if (technicalReview.CarId != carId) throw new ContentNotFoundException($"Provided car id is wrong [car id: {carId}])");

            var result = _mapper.Map<TechnicalReviewDto>(technicalReview);
            return result;
        }

        public IEnumerable<TechnicalReviewDto> GetAll(int carId)
        {
            var car = GetCarById(carId);

            var results = _mapper.Map<List<TechnicalReviewDto>>(car.TechnicalReviews);
            return results;

        }

        public void DeleteAll(int carId)
        {
            _logger.LogWarning($"Technical reviews delete action invoked (car id: {carId}) by user with an Id: {_userContextService.UserId}");
            var car = GetCarById(carId);
            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, car, new ResourceOperationRequirement(ResourceOperationType.Delete)).Result;
            if (!authResult.Succeeded)
            {
                throw new ForbiddenException("Permission denied");
            }      
            _context.RemoveRange(car.TechnicalReviews);
            _context.SaveChanges();
            _logger.LogWarning($"Technical reviews deleted (car id: {carId}) by user with an Id: {_userContextService.UserId}");
        }
        private Car GetCarById(int carId)
        {
            var car = _context.Cars
              .Include(c => c.TechnicalReviews)
              .FirstOrDefault(c => c.Id == carId);

            if (car is null) throw new ContentNotFoundException($"Car with id: {carId} was not found");

            return car;
        }
        private TechnicalReview GetTechnicalReviewById(int technicalReviewId)
        {
            var technicalReview = _context.TechnicalReviews.FirstOrDefault(t => t.Id == technicalReviewId);
            if (technicalReview is null) throw new ContentNotFoundException($"Technical review with id: {technicalReviewId} was not found");
            return technicalReview;            
        }
        public void DeleteById(int carId, int technicalReviewId)
        {
            _logger.LogWarning($"Technical review with id: {technicalReviewId} delete action invoked (car id: {carId}) by user with an Id: {_userContextService.UserId}");
            var car = GetCarById(carId);
            var technicalReview = GetTechnicalReviewById(technicalReviewId);
            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, technicalReview, new ResourceOperationRequirement(ResourceOperationType.Delete)).Result;
            if (!authResult.Succeeded)
            {
                throw new ForbiddenException("Permission denied");
            }
            if (technicalReview.CarId != carId) throw new ContentNotFoundException($"Provided car id is wrong (car id: {carId})");

            _context.TechnicalReviews.Remove(technicalReview);
            _context.SaveChanges();
            _logger.LogWarning($"Technical review deleted (technical review id: {technicalReview.Id}) by user with an Id: {_userContextService.UserId}");
        }

        public void UpdateTechnicalReview(int carId, int technicalReviewId, UpdateTechnicalReviewDto dto)
        {
            var car = GetCarById(carId);
            var technicalReview = GetTechnicalReviewById(technicalReviewId);
            if (technicalReview.CarId != carId) throw new ContentNotFoundException($"Provided car id is wrong (car id: {carId})");
            var authResult = _authorizationService.AuthorizeAsync(_userContextService.User, technicalReview, new ResourceOperationRequirement(ResourceOperationType.Update)).Result;
            if (!authResult.Succeeded)
            {
                throw new ForbiddenException("Permission denied");
            }
            _mapper.Map(dto, technicalReview);

            technicalReview.CarId = carId;

            _context.SaveChanges();
            _logger.LogWarning($"Technical review with id: {technicalReviewId}  has been updated (car id: {carId}) by user with an Id:  {_userContextService.UserId}");


        }
    }
}
