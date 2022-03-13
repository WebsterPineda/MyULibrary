using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryAPI.Models
{
    public class Book : AuditModel
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int PublishedYear { get; set; }
        [Required]
        public string Genre { get; set; }
        public virtual Stock Stock { get; set; }
    }
}