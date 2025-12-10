# Customer Order API

A .NET 8 Web API for managing customers, orders, and products.

## Tech Stack

- .NET 8
- Entity Framework Core (InMemory)
- Swagger/OpenAPI

## Getting Started

```bash
# Restore dependencies
dotnet restore

# Run the application
dotnet run

# Access Swagger UI
# https://localhost:5001/swagger
```

## Project Structure

```
CustomerOrderApi/
├── Data/
│   └── AppDbContext.cs
├── Models/
│   ├── Customer.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   └── Product.cs
├── Program.cs
└── CustomerOrderApi.csproj
```

## Domain Models

### Customer

```csharp
public class Customer
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Order> Orders { get; set; }
}
```

### Order

```csharp
public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public ICollection<OrderItem> Items { get; set; }
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
```

### OrderItem

```csharp
public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
```

### Product

```csharp
public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}
```

## Database Context

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Product> Products => Set<Product>();
}
```

## Configuration

The application uses an in-memory database for development. Entity relationships are configured with:

- Unique index on Customer.Email
- Customer -> Orders (one-to-many)
- Order -> OrderItems (one-to-many)
- OrderItem -> Product (many-to-one)
