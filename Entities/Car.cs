using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarAPI.Entities
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string BrandName { get; set; }
        [Required]
        public string ModelName { get; set; }
        [Required]
        public int ProductionYear { get; set; }
        public double Mileage { get; set; }
        [Required]
        public BodyType BodyType { get; set; }
        [Required]
        public virtual Engine Engine { get; set; }
        [Required]
        public virtual Insurance OcInsurance { get; set; }
        public virtual List<Repair> CarRepairs { get; set; }
        public virtual List<TechnicalReview> TechnicalReviews { get; set; }

    }
}
