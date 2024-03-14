using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime? RepairDate { get; set; }
        public decimal RepairCost { get; set;}
        [ForeignKey(nameof(CarId))]
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        [ForeignKey(nameof(AddedByUserId))]
        public int? AddedByUserId { get; set; }
        public virtual User? AddedByUser { get; set; }
    }
}
