using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey(nameof(CarId))]
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        [ForeignKey(nameof(AddedByUserId))]
        public int? AddedByUserId { get; set; }
        public virtual User? AddedByUser { get; set; }
    }
}