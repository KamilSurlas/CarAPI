using CarAPI.Enums;

namespace CarAPI.Models
{
    public class UpdateCarDto
    {
        public double Mileage { get; set; }
        public int EngineHorsepower { get; set; }
        public decimal EngineDisplacement { get; set; }
        public FuelType FuelType { get; set; }
    }
}
