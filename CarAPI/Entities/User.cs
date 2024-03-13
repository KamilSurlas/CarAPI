using System.ComponentModel.DataAnnotations;

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
        public int RoleId { get; set; }
        public virtual Role Role{ get; set; }
    }
}
