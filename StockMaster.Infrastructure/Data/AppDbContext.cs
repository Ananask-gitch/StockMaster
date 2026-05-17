using Microsoft.EntityFrameworkCore;
using StockMaster.Domain.Entities;

namespace StockMaster.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options){}
    public DbSet<Product> Products { get; set; }
}