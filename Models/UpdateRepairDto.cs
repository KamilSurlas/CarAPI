using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class UpdateRepairDto
    {
        public string? Description { get; set; }

        public DateTime? RepairDate { get; set; }
     
        public decimal? RepairCost { get; set; }
    }
}
