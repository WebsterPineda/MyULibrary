using System.ComponentModel.DataAnnotations;

namespace MyLibraryAPI.Models
{
    public class UserView
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
    }
}