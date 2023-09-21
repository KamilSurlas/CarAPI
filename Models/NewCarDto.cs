﻿using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class NewCarDto
    {
        [Required]
        [MaxLength(30)]
        public string BrandName { get; set; }
        [Required]
        [MaxLength(30)]
        public string ModelName { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        public int ProductionYear { get; set; }
        [Required]
        public double Mileage { get; set; }
        [Required]
        public BodyType BodyType { get; set; }
        [Required]
        public int EngineHorsepower { get; set; }
        [Required]
        public decimal EngineDisplacement { get; set; }
        [Required]
        public FuelType FuelType { get; set; }
        [Required]
        public Drivetrain Drivetrain { get; set; }
        [Required]
        public DateTime OcInsuranceStartDate { get; set; }
        [Required]
        public DateTime OcInsuranceEndDate { get; set; }
        [Required]
        public string OcPolicyNumber { get; set; }
    }
}
