namespace RedisPatterns.Application.Interfaces;

public interface IProductPurchaseService
{
    Task<bool> PurchaseProductAsync(int productId, int quantity);
}