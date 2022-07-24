using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Author
    {
        [Key]
        public Guid AuthorId { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public ICollection<Book> Books { get; set; }

    }
}
