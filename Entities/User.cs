using System.ComponentModel.DataAnnotations;

namespace CarAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string HashedPassword { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role{ get; set; }
    }
}
