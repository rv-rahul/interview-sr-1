using CustomerOrderApi.Models;
using CustomerOrderApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _svc;

    public ProductsController(ProductService svc)
    {
        _svc = svc;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> Get()
    {
        return Ok(await _svc.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Get(int id)
    {
        var p = await _svc.Get(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(
        [FromQuery] string name,
        [FromQuery] string? description,
        [FromQuery] decimal price,
        [FromQuery] int stock)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Name required");
        }

        var product = await _svc.Create(name, description ?? "", price, stock);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id}/stock")]
    public async Task<ActionResult> UpdateStock(int id, [FromBody] int quantity)
    {
        var result = await _svc.UpdateStock(id, quantity);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("internal/context")]
    public ActionResult GetContext()
    {
        var ctx = _svc.GetDbContext();
        return Ok(new { provider = ctx.Database.ProviderName, canConnect = ctx.Database.CanConnect() });
    }
}
