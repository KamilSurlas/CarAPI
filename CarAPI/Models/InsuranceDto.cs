using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class InsuranceDto
    {
        public DateTime StartDate { get; set; }
  
        public DateTime EndDate { get; set; }
  
        public string PolicyNumber { get; set; }
   
    }
}
