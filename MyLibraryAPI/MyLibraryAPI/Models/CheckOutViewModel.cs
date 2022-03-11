using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Models
{
    public class CheckOutViewModel
    {
        public int CheckOutId { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int PublishedYear { get; set; }
        public DateTime Since { get; set; }
        public string StudentName { get; set; }
    }
}