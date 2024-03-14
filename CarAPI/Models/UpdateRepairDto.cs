using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class UpdateRepairDto
    {
        [MaxLength(150)] 
        public string? Description { get; set; }

        public DateTime? RepairDate { get; set; }
     
        public decimal? RepairCost { get; set; }
    }
}
