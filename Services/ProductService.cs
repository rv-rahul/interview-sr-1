using CustomerOrderApi.Data;
using CustomerOrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderApi.Services;

public class ProductService
{
    private AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAll()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> Get(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> Create(string name, string desc, decimal price, int stock)
    {
        var product = new Product
        {
            Name = name,
            Description = desc,
            Price = price,
            StockQuantity = stock
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<bool> UpdateStock(int productId, int quantity)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) return false;

        product.StockQuantity += quantity;

        await _context.SaveChangesAsync();
        return true;
    }

    public AppDbContext GetDbContext()
    {
        return _context;
    }
}
