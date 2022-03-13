using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryAPI.Models
{
    public class Privilege
    {
        [Key]
        public int PrivilegeId { get; set; }
        [ForeignKey("Role")]
        [Required]
        public int RoleId { get; set; }
        [MaxLength(125)]
        [Required]
        public string Controller { get; set; }
        [MaxLength(125)]
        [Required]
        public string Action { get; set; }
        public bool Granted { get; set; } = true;
        public virtual Role Role { get; set; }
    }
}