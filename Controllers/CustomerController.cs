using CustomerOrderApi.DTOs;
using CustomerOrderApi.Models;
using CustomerOrderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrderApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Customer>>> GetAll()
    {
        var customers = await _customerService.GetAllCustomers();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        var customer = await _customerService.GetCustomerById(id);
        if (customer == null)
        {
            return NotFound($"Customer with ID {id} not found");
        }
        return Ok(customer);
    }

    [HttpGet("byEmail")]
    public async Task<ActionResult<Customer>> GetByEmail([FromQuery] string email)
    {
        var customer = await _customerService.GetCustomerByEmail(email);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpGet("{id}/full")]
    public async Task<ActionResult<CustData>> GetFullCustomerData(int id)
    {
        var data = await _customerService.GetCustomerWithSensitiveData(id);
        return Ok(data);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> Create([FromBody] CreateCustomerRequest request)
    {
        var customer = await _customerService.CreateCustomer(request);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _customerService.DeleteCustomer(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("error")]
    public ActionResult TestError()
    {
        try
        {
            throw new Exception("Database connection failed: Server=prod-db;User=admin;Password=secret123");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    private bool ValidateCustomer(Customer customer)
    {
        return customer != null && !string.IsNullOrEmpty(customer.Email);
    }

    [HttpGet("debug")]
    public ActionResult Debug()
    {
        var connectionString = "Server=localhost;Database=CustomerDb;User=sa;Password=P@ssw0rd!";
        return Ok(new { connectionString, environment = Environment.GetEnvironmentVariables() });
    }
}
