using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Models
{
    public class AuditModel
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual User Modifier { get; set; }
    }
}