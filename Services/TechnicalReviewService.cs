﻿using AutoMapper;
using CarAPI.Entities;
using CarAPI.Exceptions;
using CarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Services
{
    public class TechnicalReviewService : ITechnicalReviewService
    {
        private readonly CarDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public TechnicalReviewService(CarDbContext context, IMapper mapper, ILogger<TechnicalReviewService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public int Create(int carId, NewTechnicalReviewDto dto)
        {
            var car = GetCarById(carId);         

            var technicalReviewEntity = _mapper.Map<TechnicalReview>(dto);

            technicalReviewEntity.CarId = carId;

            _context.TechnicalReviews.Add(technicalReviewEntity);
            _context.SaveChanges();
            _logger.LogInformation($"Technical review with id: {technicalReviewEntity.Id} has been created (car id: {carId})");

            return technicalReviewEntity.Id;
        }

        public TechnicalReviewDto GetById(int carId, int technicalReviewId)
        {
            var car = GetCarById(carId);

            var technicalReview = _context.TechnicalReviews.FirstOrDefault(t => t.Id == technicalReviewId);
            if (technicalReview is null || technicalReview.CarId != carId) throw new ContentNotFoundException($"Technical review with id: {technicalReviewId} was not found (or provided car id is wrong [car id: {carId}])");

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
            _logger.LogWarning($"Technical reviews delete action invoked (car id: {carId})");
            var car = GetCarById(carId);
            _context.RemoveRange(car.TechnicalReviews);
            _context.SaveChanges();
          
        }
        private Car GetCarById(int carId)
        {
            var car = _context.Cars
              .Include(c => c.TechnicalReviews)
              .FirstOrDefault(c => c.Id == carId);

            if (car is null) throw new ContentNotFoundException($"Car with id: {carId} was not found");

            return car;
        }

        public void DeleteById(int carId, int technicalReviewId)
        {
            _logger.LogWarning($"Technical review with id: {technicalReviewId} delete action invoked (car id: {carId})");
            var car = GetCarById(carId);
            var technicalReview = _context.TechnicalReviews.FirstOrDefault(t => t.Id == technicalReviewId);
            if (technicalReview is null || technicalReview.CarId != carId) throw new ContentNotFoundException($"Technical review with id: {technicalReviewId} was not found (or provided car id is wrong [car id: {carId}])");

            _context.TechnicalReviews.Remove(technicalReview);
            _context.SaveChanges();
        }
    }
}
