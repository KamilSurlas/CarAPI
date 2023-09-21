using CarAPI.Enums;

namespace CarAPI.Models
{
    public class UpdateTechnicalReviewDto 
    {
        public DateTime TechnicalReviewDate { get; set; }
        public string? Description { get; set; }
        public TechnicalReviewResult TechnicalReviewResult { get; set; }
        public int CarId { get; set; }
    }
}
