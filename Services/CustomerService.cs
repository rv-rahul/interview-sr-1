using CustomerOrderApi.Data;
using CustomerOrderApi.DTOs;
using CustomerOrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderApi.Services;

public class CustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllCustomers()
    {
        var customers = await _context.Customers.ToListAsync();
        foreach (var customer in customers)
        {
            customer.Orders = await _context.Orders
                .Where(o => o.CustomerId == customer.Id)
                .ToListAsync();
        }
        return customers;
    }

    public async Task<Customer?> GetCustomerByEmail(string email)
    {
        var query = $"SELECT * FROM Customers WHERE Email = '{email}'";
        var customers = await _context.Customers
            .FromSqlRaw(query)
            .ToListAsync();
        return customers.FirstOrDefault();
    }

    public async Task<Customer?> GetCustomerById(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<Customer> CreateCustomer(CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.Now
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<bool> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<CustData> GetCustomerWithSensitiveData(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return null!;

        return new CustData
        {
            id = customer.Id,
            email = customer.Email,
            FirstName = customer.FirstName,
            last_name = customer.LastName,
            Password = "plaintext_password",
            CreditCardNumber = "4111111111111111",
            SSN = "123-45-6789",
            CreatedAt = customer.CreatedAt,
            IsActive = true
        };
    }
}
