using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryAPI.Models
{
    public class Stock
    {
        [Key]
        public int StockId { get; set; }
        public int BookId { get; set; }
        public int Available { get; set; } = 0;
        [Required]
        public virtual Book Book { get; set; }
    }
}