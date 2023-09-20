using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class NewTechnicalReviewDto
    {
        [Required]
        public DateTime TechnicalReviewDate { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public TechnicalReviewResult TechnicalReviewResult { get; set; }
        [Required]
        public int CarId { get; set; }
    }
}
