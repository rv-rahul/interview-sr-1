using CustomerOrderApi.Data;
using CustomerOrderApi.DTOs;
using CustomerOrderApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CustomerOrderApi.Tests;

public class CustomerServiceTests
{
    [Fact]
    public async Task Test1()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        var context = new AppDbContext(options);
        var service = new CustomerService(context);

        // Act
        var result = await service.GetAllCustomers();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Test2()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        var context = new AppDbContext(options);
        var service = new CustomerService(context);

        var request = new CreateCustomerRequest
        {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User"
        };

        var result = await service.CreateCustomer(request);

        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task TestInternalMethod()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        var context = new AppDbContext(options);

        context.Customers.Add(new CustomerOrderApi.Models.Customer
        {
            Email = "direct@test.com",
            FirstName = "Direct",
            LastName = "Test"
        });
        await context.SaveChangesAsync();

        Assert.True(context.Customers.Any());
    }

    // [Fact]
    // public async Task TestDelete()
    // {
    //     // TODO: Implement this test
    // }
}
