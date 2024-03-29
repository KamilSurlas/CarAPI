﻿using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class NewTechnicalReviewDto
    {
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        [Required]
        public DateTime? TechnicalReviewDate { get; set; }
        [Required]
        public TechnicalReviewResult? TechnicalReviewResult { get; set; }
    }
}
