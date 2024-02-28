using CarAPI.Entities;
using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class TechnicalReviewDto
    {
        public int Id { get; set; }
        public DateTime TechnicalReviewDate { get; set; }

        public string Description { get; set; }
   
        public TechnicalReviewResult TechnicalReviewResult { get; set; }
   
    }
}
