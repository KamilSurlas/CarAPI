using CarAPI.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarAPI.Entities
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string BrandName { get; set; }
        [Required]
        [MaxLength(15)]
        public string RegistrationNumber { get; set; }
        [Required]
        [MaxLength(30)]
        public string ModelName { get; set; }
        public int ProductionYear { get; set; }
        public double Mileage { get; set; } 
        [Required]
        public BodyType? BodyType { get; set; }
        [Required]
        public Drivetrain? Drivetrain { get; set; }
        [Required]
        public virtual Engine Engine { get; set; }
        [Required]
        public virtual Insurance OcInsurance { get; set; }
        public virtual List<Repair> CarRepairs { get; set; }
        public virtual List<TechnicalReview> TechnicalReviews { get; set; }
        public int? CreatedByUserId { get; set; }
        public virtual User? CreatedByUser { get; set; }
    }
}
