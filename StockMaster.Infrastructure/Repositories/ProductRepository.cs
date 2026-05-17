using Microsoft.EntityFrameworkCore;
using StockMaster.Infrastructure.Data;
using StockMaster.Domain.Entities;
using StockMaster.Application.Interfaces;

namespace StockMaster.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository (AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync() =>
        await _context.Products.ToListAsync();

    public async Task<List<Product>?> GetByIdsAsync(List<int> ids) =>
        await _context.Products.
        Where(p=>ids.Contains(p.Id)).
        ToListAsync();
    public async Task AddRangeProductsAsync(List<Product> products) =>
        await _context.Products.AddRangeAsync(products);
    public Task UpdateRangeAsync(List<Product> products) 
    {
        _context.Products.UpdateRange(products);
        return Task.CompletedTask;
    }
    public async Task DeleteRangeProductsAsync(List<int> ids)
    {
        var product = await GetByIdsAsync(ids);
        if (product != null)
            _context.Products.RemoveRange(product);
    }
    public async Task<bool> SaveChangesAsync() =>
        await _context.SaveChangesAsync() > 0;

}