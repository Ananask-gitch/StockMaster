using StockMaster.Domain.Entities;

namespace StockMaster.Application.Interfaces;

public interface IProductRepository
    {
    Task<List<Product>> GetAllAsync();
    Task<List<Product>?> GetByIdsAsync(List<int> id);
    Task AddRangeProductsAsync(List<Product> products);
    Task UpdateRangeAsync(List<Product> products);
    Task DeleteRangeProductsAsync(List<int> id);
    Task<bool> SaveChangesAsync();
    }
