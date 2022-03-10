using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Models
{
    public class CheckOut 
    {
        public int CheckOutId { get; set; }
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public bool Returned { get; set; }
        public DateTime CheckedOutMoment { get; set; } = DateTime.Now;
        [ForeignKey("StudentId")]
        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual Book Book { get; set; }
    }
}