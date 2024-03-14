using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class UpdateCarDto
    {
        public double? Mileage { get; set; }
        [MaxLength(15)]
        public string? RegistrationNumber { get; set; }
        public Drivetrain? Drivetrain { get; set; }

    }
}
