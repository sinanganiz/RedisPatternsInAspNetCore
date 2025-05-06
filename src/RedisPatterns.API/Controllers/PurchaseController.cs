using Microsoft.AspNetCore.Mvc;
using RedisPatterns.Application.Interfaces;

namespace RedisPatterns.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseController : ControllerBase
{
    private readonly IProductPurchaseService _purchaseService;

    public PurchaseController(IProductPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpPost]
    public async Task<IActionResult> Purchase([FromQuery] int productId, [FromQuery] int quantity)
    {
        var success = await _purchaseService.PurchaseProductAsync(productId, quantity);
        if (!success)
            return BadRequest("Purchase failed. Possible race condition or insufficient stock.");

        return Ok("Purchase successful.");
    }
}