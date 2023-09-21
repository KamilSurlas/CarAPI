using CarAPI.Models;

namespace CarAPI.Services
{
    public interface ITechnicalReviewService
    {
        int Create(int carId, NewTechnicalReviewDto dto);
        TechnicalReviewDto GetById(int carId, int technicalReviewId);
        IEnumerable<TechnicalReviewDto> GetAll(int carId);
        void DeleteAll(int carId);
        void DeleteById(int carId, int technicalReviewId);
        void UpdateTechnicalReview(int carId, int technichalReviewId, UpdateTechnicalReviewDto dto);
    }
}