using CustomerOrderApi.DTOs;
using CustomerOrderApi.Models;
using CustomerOrderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrderApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly CustomerService _customerService;

    public OrdersController(OrderService orderService, CustomerService customerService)
    {
        _orderService = orderService;
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrders();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _orderService.GetOrder(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost]
    public Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        return CreateOrderInternal(request);
    }

    private async Task<ActionResult<Order>> CreateOrderInternal(CreateOrderRequest request)
    {
        var order = await _orderService.CreateOrder(request);
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult> UpdateStatus(int id, [FromQuery] string status)
    {
        var result = await _orderService.UpdateOrderStatus(id, status);
        if (!result)
        {
            return BadRequest("Failed to update status");
        }
        return NoContent();
    }

    [HttpGet("export")]
    public async Task<ActionResult<string>> ExportOrders()
    {
        var json = await _orderService.ExportOrdersToJson();
        return Ok(json);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Order>>> Search([FromQuery] string q)
    {
        Console.WriteLine($"Searching for: {q}");
        var results = await _orderService.SearchOrders(q);
        return Ok(results);
    }

    [HttpGet("{id}/items")]
    public async Task<ActionResult> GetOrderItems(int id)
    {
        try
        {
            var order = await _orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order.Items);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            return StatusCode(500, "An error occurred");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> CancelOrder(int id)
    {
        var order = await _orderService.GetOrder(id);
        if (order == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}
