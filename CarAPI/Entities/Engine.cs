﻿using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CarAPI.Entities
{
    public class Engine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int? Horsepower { get; set; }
        [Required]
        public decimal? Displacement  {get; set; }
        [Required]
        public FuelType? FuelType { get; set; }
        [ForeignKey(nameof(CarId))]
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
    }
}
