using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(150)]
        public string FirstName { get; set; }
        [MaxLength(300)]
        public string LastName { get; set; }
        [Required]
        [Index( "Unq_UsrEmail", IsUnique = true)]
        [MaxLength(500)]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool TempPassword { get; set; } = false;
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}