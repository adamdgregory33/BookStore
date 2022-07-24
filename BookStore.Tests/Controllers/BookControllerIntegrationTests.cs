using BookStore.Models;
using BookStore.Tests.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests
{
    public class BookControllerIntegrationTests : BaseIntegrationTest
    {
        private const string Endpoint = "api/Books";

        [Fact]
        public async Task Post_BookAdded_SuccessResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var book = TestExtensions.CreateBook();
            var stringContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            
            // Act - Post a book object
            var postResponse = await client.PostAsync(Endpoint, stringContent);

            // Assert
            postResponse.EnsureSuccessStatusCode(); 
            Assert.Equal("application/json; charset=utf-8",
                postResponse.Content.Headers.ContentType.ToString());

            var responseBook = await postResponse.Content.ReadAsAsync<Book>();

            Assert.Equal(book.BookId, responseBook.BookId);
        }

        [Fact]
        public async Task Put_BookUpdated_SuccessResponse()
        {
            // Arrange
            var client = factory.CreateClient();
            var book = TestExtensions.CreateBook();

            var stringContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

            // Act - Put an existing book object
            await client.PostAsync(Endpoint, stringContent);

            book.Title = "Different";
            stringContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

            var putResponse = await client.PutAsync($"{Endpoint}/{book.BookId}", stringContent);

            // Assert - Gets updated successfully
            putResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                putResponse.Content.Headers.ContentType.ToString());

            var updatedBook = await putResponse.Content.ReadAsAsync<Book>();

            Assert.Equal(book.BookId, updatedBook.BookId);
        }

        [Fact]
        public async Task Get_SingleId_SuccessResponse()
        {
            // Arrange
            var book = TestExtensions.CreateBook();

            var stringContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

            // Act - Get an existing book object
            using var client = factory.CreateClient();
            
                await client.PostAsync(Endpoint, stringContent);

                var getResponse = await client.GetAsync($"{Endpoint}/{book.BookId}");
                getResponse.EnsureSuccessStatusCode();

                // Assert
                Assert.Equal("application/json; charset=utf-8",
                    getResponse.Content.Headers.ContentType.ToString());

                var getBook = await getResponse.Content.ReadAsAsync<Book>();
                Assert.Equal(book.BookId, getBook.BookId);
        }

        [Fact]
        public async Task Get_ListAll_SuccessResponse()
        {
            // Arrange
            using var client = factory.CreateClient();
            var books = new List<Book>()
            {
                TestExtensions.CreateBook(),
                TestExtensions.CreateBook()
            };

            foreach(var book in books)
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
                await client.PostAsync(Endpoint, stringContent);
            }

            // Act - Get existing book objects
            var getResponse = await client.GetAsync(Endpoint);

            // Assert - Gets all
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", getResponse.Content.Headers.ContentType.ToString());
            var getBooks = JsonConvert.DeserializeObject<List<Book>>(await getResponse.Content.ReadAsStringAsync());
            Assert.Equal(2, getBooks.Count);
        }
    }
}