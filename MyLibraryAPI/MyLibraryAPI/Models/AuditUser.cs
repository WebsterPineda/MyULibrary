using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Models
{
    public class AuditUser
    {
        [Key]
        public int AuditUserId { get; set; }
        public int ExecutorUserId { get; set; }
        public int AfectedUserId { get; set; }
        [Required]
        public string Action { get; set; }
        [ForeignKey("ExecutorUserId")]
        public virtual User ExecutorUser { get; set; }
        [ForeignKey("AfectedUserId")]
        public virtual User AfectedUser { get; set; }
    }
}