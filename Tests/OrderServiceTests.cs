using CustomerOrderApi.Data;
using CustomerOrderApi.DTOs;
using CustomerOrderApi.Models;
using CustomerOrderApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CustomerOrderApi.Tests;

public class OrderServiceTests
{
    [Fact]
    public void Placeholder()
    {
        Assert.True(true);
    }

    [Fact]
    public void OrderModel_ShouldHaveProperties()
    {
        var order = new Order
        {
            Id = 1,
            CustomerId = 1,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Pending
        };

        Assert.Equal(1, order.Id);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }
}
