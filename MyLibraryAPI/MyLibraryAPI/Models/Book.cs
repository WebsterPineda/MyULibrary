using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Models
{
    public class Book : AuditModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
        public string Genre { get; set; }
        public virtual Stock Stock { get; set; }
    }
}