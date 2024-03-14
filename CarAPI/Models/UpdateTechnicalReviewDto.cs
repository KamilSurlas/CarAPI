using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class UpdateTechnicalReviewDto 
    {
        public DateTime? TechnicalReviewDate { get; set; }
        [MaxLength(150)]
        public string? Description { get; set; }
        public TechnicalReviewResult? TechnicalReviewResult { get; set; }
    }
}
