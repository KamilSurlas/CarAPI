using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAPI.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)] 
        public string LastName { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        public string HashedPassword { get; set; }
        [ForeignKey(nameof(RoleId))]
        public int RoleId { get; set; } = 1;
        public virtual Role Role{ get; set; }
    }
}
