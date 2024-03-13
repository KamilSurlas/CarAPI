using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarAPI.Entities
{
    public class TechnicalReview
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime? TechnicalReviewDate { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        [Required]
        public TechnicalReviewResult? TechnicalReviewResult { get; set; }
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public int? AddedByUserId { get; set; }
        public virtual User? AddedByUser { get; set; }
    }
}