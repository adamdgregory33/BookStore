using BookStore.Controllers;
using BookStore.Data;
using BookStore.Data.Repository;
using BookStore.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using BookStore.Models;
using Xunit.Abstractions;
using System;

namespace BookStore.Tests
{
    public class BookRepositoryTests
    {
        protected readonly ITestOutputHelper output;

        public BookRepositoryTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private DbContextOptions<BookStoreContext> GetTestDbContextOptions(string tempDbName)
        {
            return new DbContextOptionsBuilder<BookStoreContext>()
                .UseInMemoryDatabase(tempDbName)
                .Options;
        }

        [Fact]
        public async Task Save_SavesBook()
        {
            // Given - A Book
            var options = GetTestDbContextOptions(Guid.NewGuid().ToString());

            var book1 = TestExtensions.CreateBook();
            var book2 = TestExtensions.CreateBook();

            // When - Saving books
            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);
                await repository.Add(book1);
                await repository.Add(book2);
            }

            // Then - DB Contains books
            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);
                (await context.Books.CountAsync()).Should().Be(2);
            }
        }

        [Fact]
        public async Task Update_UpdatesBookWithId()
        {
            // Given - A Book
            var options = GetTestDbContextOptions(Guid.NewGuid().ToString());

            var book1 = TestExtensions.CreateBook();
            var book2 = TestExtensions.CreateBook();

            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);
                await repository.Add(book1);
                await repository.Add(book2);
            }

            // When - Updating a book
            var newTitle = "New";
            book1.Title = newTitle;
            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);
                await repository.Update(book1);
            }

            // Then - DB Contains Updated Book
            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);
                (await repository.Get(book1.BookId)).Title.Should().Be(newTitle);
            }
        }

        [Fact]
        public async Task Delete_DeletesBookWithId()
        {
            // Given - A Book
            var options = GetTestDbContextOptions(Guid.NewGuid().ToString());

            var book1 = TestExtensions.CreateBook();
            var book2 = TestExtensions.CreateBook();

            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);
                await repository.Add(book1);
                await repository.Add(book2);
            }

            // When - Deleting a book
            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);
                await repository.Delete(book1.BookId);

                // Then - Should be retreivable
                (await repository.Get(book1.BookId)).Should().BeNull();
            }
        }

        [Fact]
        public async Task Get_FindsBookWithId()
        {
            // Given - A Book
            var options = GetTestDbContextOptions(Guid.NewGuid().ToString());

            var book1 = TestExtensions.CreateBook();
            var book2 = TestExtensions.CreateBook();

            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);
                await repository.Add(book1);
                await repository.Add(book2);
            }

            // When - Finding a book
            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);

                // Then - Should be retreivable
                (await repository.Get(book2.BookId)).Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Get_FindsAllBooks()
        {
            // Given - A Book
            var options = GetTestDbContextOptions(Guid.NewGuid().ToString());

            var book1 = TestExtensions.CreateBook();
            var book2 = TestExtensions.CreateBook();

            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);
                await repository.Add(book1);
                await repository.Add(book2);
            }

            // When - Finding a book
            using (var context = new BookStoreContext(options))
            {
                var repository = new BookRepository(context);
                var books = await repository.GetAll();

                // Then - Should be retreivable
                books.Should().HaveCount(2);
            }
        }
    }
}