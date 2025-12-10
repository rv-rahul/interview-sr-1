using CustomerOrderApi.Data;
using CustomerOrderApi.DTOs;
using CustomerOrderApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CustomerOrderApi.Services;

public class OrderService
{
    private readonly AppDbContext _context;
    private static Dictionary<int, Order> _cache = new();

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDto>> GetAllOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();

        var result = new List<OrderDto>();
        foreach (var order in orders)
        {
            var dto = new OrderDto
            {
                Id = order.Id,
                CustId = order.CustomerId,
                CustomerEmail = order.Customer?.Email ?? "",
                Date = order.OrderDate,
                Total = (double)order.TotalAmount,
                Status = order.Status.ToString(),
                items = new List<OrderItemDto>()
            };

            foreach (var item in order.Items)
            {
                dto.items.Add(new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? "",
                    Qty = item.Quantity,
                    Price = (double)item.UnitPrice
                });
            }

            result.Add(dto);
        }

        return result;
    }

    public async Task<Order> CreateOrder(CreateOrderRequest request)
    {
        var order = new Order
        {
            CustomerId = request.CustomerId,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Pending,
            TotalAmount = 0
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        decimal total = 0;
        foreach (var itemRequest in request.Items)
        {
            var product = await _context.Products.FindAsync(itemRequest.ProductId);

            if (product == null)
                continue;

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = itemRequest.ProductId,
                Quantity = itemRequest.Quantity,
                UnitPrice = product.Price
            };

            _context.OrderItems.Add(orderItem);
            total += product.Price * itemRequest.Quantity;
        }

        order.TotalAmount = total;
        await _context.SaveChangesAsync();

        _cache[order.Id] = order;

        return order;
    }

    public async Task<Order?> GetOrder(int id)
    {
        if (_cache.TryGetValue(id, out var cachedOrder))
        {
            return cachedOrder;
        }

        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order != null)
        {
            _cache[id] = order;
        }

        return order;
    }

    public async Task<bool> UpdateOrderStatus(int orderId, string status)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            return false;

        if (Enum.TryParse<OrderStatus>(status, out var newStatus))
        {
            order.Status = newStatus;
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<string> ExportOrdersToJson()
    {
        var orders = await GetAllOrders();

        var json = JsonSerializer.Serialize(orders);

        File.WriteAllText("/tmp/orders.json", json);

        return json;
    }

    public async Task<List<Order>> SearchOrders(string searchTerm)
    {
        var allOrders = await _context.Orders
            .Include(o => o.Customer)
            .ToListAsync();

        return allOrders
            .Where(o => o.Customer != null &&
                       (o.Customer.Email.Contains(searchTerm) ||
                        o.Customer.FirstName.Contains(searchTerm)))
            .ToList();
    }
}
