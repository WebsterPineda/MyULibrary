using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryAPI.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [MaxLength(150)]
        [Index(IsUnique = true)]
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set;  }
        public virtual ICollection<Privilege> Privileges { get; set; }
    }
}