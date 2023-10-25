using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class UpdateInsuranceDto
    {     
        public DateTime StartDate { get; set; }
       
        public DateTime EndDate { get; set; }
       
        public string PolicyNumber { get; set; }
    }
}
