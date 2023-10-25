using CarAPI.Entities;
using CarAPI.Enums;

namespace CarAPI.Models
{
    public class CarDto
    {
        public int Id { get; set; }
      
        public string BrandName { get; set; }
    
        public string ModelName { get; set; }
        public string RegistrationNumber { get; set; }

        public int ProductionYear { get; set; }
        public double Mileage { get; set; }
        
        public BodyType BodyType { get; set; }
        public Drivetrain Drivetrain { get; set; }

        public int EngineHorsepower { get; set; }
        public decimal EngineDisplacement { get; set; }
        public FuelType FuelType { get; set; }
        public virtual InsuranceDto OcInsurance { get; set; }
        public virtual List<RepairDto> CarRepairs { get; set; }
        public virtual List<TechnicalReviewDto> TechnicalReviews { get; set; }

    }
}
