using CarAPI.Enums;

namespace CarAPI.Models
{
    public class UpdateCarDto
    {
        public double? Mileage { get; set; }
        public string? RegistrationNumber { get; set; }
        public Drivetrain? Drivetrain { get; set; }

    }
}
