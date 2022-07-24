using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repository
{
    public class BookRepository : IDataRepository<Book>
    {

        private readonly BookStoreContext context;

        public BookRepository(BookStoreContext context) => this.context = context;

        public async Task Add(Book book)
        {
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
        }

        public async Task Update(Book book)
        {
            var entity = await context.Books.FirstOrDefaultAsync(x => x.BookId == book.BookId);
            context.Entry(entity).CurrentValues.SetValues(book);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<Book> Get(Guid id)
        {
            return await context.Books
                .FirstOrDefaultAsync(x => x.BookId == id);
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await context.Books
                .ToListAsync();
        }

        public async Task Delete(Guid id)
        {
            var book  = context.Set<Book>().Find(id);
            context.Set<Book>().Remove(book);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Book book)
        {
            context.Set<Book>().Remove(book);
            await context.SaveChangesAsync();
        }

    }
}
