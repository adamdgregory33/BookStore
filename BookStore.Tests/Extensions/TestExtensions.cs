using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Tests.Extensions
{
    public class TestExtensions
    {

        public static Book CreateBook()
        {
            return new Book
            {
                BookId = Guid.NewGuid(),
                Title = "Book1",
                Published = DateTime.Now
            };
        }

        
    }
}
