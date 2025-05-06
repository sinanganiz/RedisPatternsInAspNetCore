using RedisPatterns.Domain.Entities;

namespace RedisPatterns.Application.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
}