using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.Data.Repository;
using BookStore.Data;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IDataRepository<Book> bookRepository;

        public BooksController(IDataRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        // GET: api/Books
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Book>>> GetBooks()
        {
            var books = await bookRepository.GetAll();

            if (books == null || !books.Any())
                return NotFound();          

            return Ok( books.ToList() );
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Book>> GetBook(Guid id)
        {
            var book = await bookRepository.Get(id);

            if(book == null)
                return NotFound();

            return Ok(book);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutBook(Guid id, Book book)
        {
            if (id != book.BookId)
                return BadRequest("Book Id does not match the Id you are trying to update");

            if(await bookRepository.Get(id) == null)
                return NotFound();

            await bookRepository.Update(book);

            return AcceptedAtAction(nameof(this.PutBook), new { id = book.BookId }, book);
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (await bookRepository.Get(book.BookId) != null)
                return BadRequest("Book Id Already Exists");

            await bookRepository.Add(book);

            return CreatedAtAction(nameof(this.GetBook), new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await bookRepository.Get(id);
            if (book == null)
                return NotFound();

            return Ok();
        }
    }
}
