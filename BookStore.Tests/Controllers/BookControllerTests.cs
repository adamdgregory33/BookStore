using BookStore.Controllers;
using BookStore.Data.Repository;
using BookStore.Models;
using BookStore.Tests.Extensions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Tests
{
    public class BookControllerTests
    {
        [Fact]
        public async Task GetAll_ValidInput_OK()
        {
            // Given valid input for api/Books
            var mockRepo = new Mock<IDataRepository<Book>>();
            mockRepo.Setup(x => x.GetAll())
                .ReturnsAsync( 
                new List<Book>{
                    TestExtensions.CreateBook()
                });

            var controller = new BooksController(mockRepo.Object);

            // When Controller is hit
            var result = await controller.GetBooks();

            // Then Respond with 200 OK response and objects
            var okObject = result.Result as OkObjectResult;
            Assert.NotNull(okObject);
        }

        [Fact]
        public async Task GetAll_ValidInput_NotFound()
        {
            // Given valid input for api/Books but no books
            var mockRepo = new Mock<IDataRepository<Book>>();
            mockRepo.Setup(x => x.GetAll())
                .ReturnsAsync(
                new List<Book>());
            var controller = new BooksController(mockRepo.Object);

            // When Controller is hit
            var result = await controller.GetBooks();

            // Then Respond with NotFound
            var notFoundObject = result.Result as NotFoundResult;
            Assert.NotNull(notFoundObject);
        }

        [Fact]
        public async Task GetById_ValidInput_OK()
        {
            // Given valid input for api/Books/{id}
            var mockRepo = new Mock<IDataRepository<Book>>();
            var book = TestExtensions.CreateBook();
            mockRepo.Setup(x => x.Get(book.BookId))
                .ReturnsAsync(book);

            var controller = new BooksController(mockRepo.Object);

            // When Controller is hit
            var result = await controller.GetBook(book.BookId);

            // Then Respond with 200 OK response and object
            var okObject = result.Result as OkObjectResult;
            Assert.NotNull(okObject);
        }

        [Fact]
        public async Task GetById_ValidInput_NotFound()
        {
            // Given valid input for api/Books/{id}
            var mockRepo = new Mock<IDataRepository<Book>>();

            var controller = new BooksController(mockRepo.Object);

            // When Controller is hit
            var result = await controller.GetBook(Guid.NewGuid());

            // Then Respond with 200 OK response and object
            var notFoundObject = result.Result as NotFoundResult;
            Assert.NotNull(notFoundObject);
        }

        [Fact]
        public async Task Put_ValidInput_AcceptedResponse()
        {
            // Given valid input for api/Books/{id}
            var mockRepo = new Mock<IDataRepository<Book>>();
            var book = TestExtensions.CreateBook();

            mockRepo.Setup(x => x.Get(book.BookId))
                .ReturnsAsync(book);

            var controller = new BooksController(mockRepo.Object);

            // When Controller is hit
            var result = await controller.PutBook(book.BookId, book);

            // Then Respond with 200 OK response and object
            var acceptedObject = result as AcceptedAtActionResult;
            Assert.NotNull(acceptedObject);
        }


        [Fact]
        public async Task Post_ValidInput_AcceptedResponse()
        {
            // Given valid input for api/Books/
            var mockRepo = new Mock<IDataRepository<Book>>();
            var book = TestExtensions.CreateBook();

            mockRepo.Setup(x => x.Get(book.BookId))
                .ReturnsAsync((Book) null);

            var controller = new BooksController(mockRepo.Object);

            // When Controller is hit
            var result = await controller.PostBook(book);

            // Then Respond with 200 OK response and object
            var createdObject = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdObject);
        }

        [Fact]
        public async Task Delete_ValidInput_AcceptedResponse()
        {
            // Given valid input for api/Books/{id}
            var mockRepo = new Mock<IDataRepository<Book>>();
            var book = TestExtensions.CreateBook();

            mockRepo.Setup(x => x.Get(book.BookId))
                .ReturnsAsync(book);

            var controller = new BooksController(mockRepo.Object);

            // When Controller is hit
            var result = await controller.DeleteBook(book.BookId);

            // Then Respond with 200 OK response
            var okObject = result as OkResult;
            Assert.NotNull(okObject);
        }

        [Fact]
        public async Task Delete_ValidInput_NotFound()
        {
            // Given valid input for api/Books/{id}
            var mockRepo = new Mock<IDataRepository<Book>>();
            var book = TestExtensions.CreateBook();

            mockRepo.Setup(x => x.Get(book.BookId))
                .ReturnsAsync((Book)null);

            var controller = new BooksController(mockRepo.Object);

            // When Controller is hit
            var result = await controller.DeleteBook(book.BookId);

            // Then Respond with 200 OK response
            var notFoundObject = result as NotFoundResult;
            Assert.NotNull(notFoundObject);
        }

    }
}