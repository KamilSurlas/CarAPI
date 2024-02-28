using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class NewInsuranceDto
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string PolicyNumber { get; set; }
    }
}