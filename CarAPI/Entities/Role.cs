using System.ComponentModel.DataAnnotations;

namespace CarAPI.Entities
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}