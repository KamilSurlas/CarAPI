using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarAPI.Entities
{
    public class TechnicalReview
    {
        public int Id { get; set; }
        [Required]
        public DateTime TechnicalReviewDate { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public TechnicalReviewResult TechnicalReviewResult { get; set; }
        [Required]
        public int CarId { get; set; }
        public virtual Car? Car { get; set; }
        public int? AddedByUserId { get; set; }
        public virtual User? AddedByUser { get; set; }
    }
}