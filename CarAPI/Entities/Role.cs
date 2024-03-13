using System.ComponentModel.DataAnnotations;

namespace CarAPI.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string RoleName { get; set; }
    }
}