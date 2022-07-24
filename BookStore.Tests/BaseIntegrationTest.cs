using BookStore.Data;
using BookStore.Data.Repository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace BookStore.Tests
{
    public class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly ITestOutputHelper output;

        public BaseIntegrationTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        protected readonly WebApplicationFactory<Program> factory;

        public BaseIntegrationTest()
        {
            factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BookStoreContext>));

                if (dbContext != null)
                    services.Remove(dbContext);

                var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<BookStoreContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTest");
                    options.UseInternalServiceProvider(serviceProvider);
                });
                services.AddScoped<IDataRepository<Book>>(serviceProvider =>
                {
                    var dbContextOptions = serviceProvider.GetService<DbContextOptions<BookStoreContext>>();
                    return new BookRepository(new BookStoreContext(dbContextOptions));
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    using (var appContext = scope.ServiceProvider.GetRequiredService<BookStoreContext>())
                    {
                        try
                        {
                            appContext.Database.EnsureCreated();
                        }
                        catch (Exception ex)
                        {
                            //Log errors
                            throw;
                        }
                    }
                }
            }));
        }

    }
}
