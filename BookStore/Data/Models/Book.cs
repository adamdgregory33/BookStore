using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        public Guid BookId { get; set; } 

        public string Title { get; set; }

        [ForeignKey("AuthorId")]
        public Guid AuthorId { get; set; }

        public DateTime Published { get; set; }
    }
}
