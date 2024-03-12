using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class NewRepairDto
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime? RepairDate { get; set; }
        [Required]
        public decimal RepairCost { get; set; }
    }
}
