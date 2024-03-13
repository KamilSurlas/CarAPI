using System.ComponentModel.DataAnnotations;

namespace CarAPI.Entities
{
    public class Repair
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        [Required]
        public DateTime RepairDate { get; set; }
        public decimal RepairCost { get; set;}
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public int? AddedByUserId { get; set; }
        public virtual User? AddedByUser { get; set; }
    }
}
