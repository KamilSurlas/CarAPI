using System.ComponentModel.DataAnnotations;

namespace CarAPI.Entities
{
    public class Insurance
    {
        public int Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string PolicyNumber { get; set; }
        [Required]    
        public virtual Car Car { get; set; }
    }
}