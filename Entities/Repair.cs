using System.ComponentModel.DataAnnotations;

namespace CarAPI.Entities
{
    public class Repair
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime RepairDate { get; set; }
        [Required]
        public decimal RepairCost { get; set;}
        [Required]
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public int? AddedByUserId { get; set; }
        public virtual User? AddedByUser { get; set; }
    }
}
