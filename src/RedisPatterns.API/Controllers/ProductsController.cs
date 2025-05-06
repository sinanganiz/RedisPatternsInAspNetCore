using Microsoft.AspNetCore.Mvc;
using RedisPatterns.Application.Interfaces;
using RedisPatterns.Domain.Entities;

namespace RedisPatterns.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> Get()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

}