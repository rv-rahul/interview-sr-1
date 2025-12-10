namespace CustomerOrderApi.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public int CustId { get; set; }
    public string CustomerEmail { get; set; } = "";
    public DateTime Date { get; set; }
    public double Total { get; set; }
    public string Status { get; set; } = "";
    public List<OrderItemDto> items { get; set; } = new();
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public int Qty { get; set; }
    public double Price { get; set; }
}

public class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public List<OrderItemRequest> Items { get; set; } = new();
}

public class OrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
