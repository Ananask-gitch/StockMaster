using StockMaster.Application.DTOs;
using StockMaster.Application.Interfaces;
using StockMaster.Domain.Entities;


namespace StockMaster.Application.Service;

public class ProductService : IProductService
{

    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductResponce>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(MapToResponce).ToList();
    }

    public async Task<List<ProductResponce>> GetByIdsAsync(List<int> ids)
    {
        if (ids is null || ids.Count == 0)
            return new List<ProductResponce>();
        var products = await _repository.GetByIdsAsync(ids);
#pragma warning disable CS8604 // Possible null reference argument.
        return products.Select(MapToResponce).ToList();
#pragma warning restore CS8604 // Possible null reference argument.
    }

    public async Task<BatchOperationResult> CreateBatchAsync(List<CreateProductRequest> request)
    {
        var result = new BatchOperationResult
        {
            TotalReceive = request.Count,
            ProcessedProducts = new List<ProductResponce>(),
            Errors = new List<BatchError>()
        };
        var validProducts = new List<Product>();
        for (int i = 0; i < request.Count; i++)
        {
            var req = request[i];
            try
            {
                var product = new Product(req.Name, req.Stock, req.Price);
                validProducts.Add(product);
                result.SuccessCount++;
            }
            catch(ArgumentException ex)
            {
                result.FailCount++;
                result.Errors.Add(new BatchError
                {
                    Index=i,
                    Identifier = req.Name,
                    Error = ex.Message,
                    ProvidedStock = req.Stock,
                    ProvidedPrice = req.Price
                });
            }
        }

        if (validProducts.Any())
        {
            await _repository.AddRangeProductsAsync(validProducts);
            await _repository.SaveChangesAsync();
        }

        result.ProcessedProducts = validProducts.Select(MapToResponce).ToList();
        return result;


    }

    public async Task<BatchOperationResult> DeleteBatchAsync(List<int> ids)
    {
        var result = new BatchOperationResult
        {
            TotalReceive = ids.Count,
            ProcessedProducts = new List<ProductResponce>(),
            Errors = new List<BatchError>()
        };

        if (ids is null || ids.Count == 0)
            return result;
        
        var existingProducts = await _repository.GetByIdsAsync(ids);
#pragma warning disable CS8604 // Possible null reference argument.
        var existingIds = existingProducts.Select(p => p.Id).ToHashSet();
#pragma warning restore CS8604 // Possible null reference argument.
        var toDeleteIds = new List<int>();

        for (int i = 0; i < ids.Count; i++)
        {
            var id = ids[i];
            if (!existingIds.Contains(id))
            {
                result.FailCount++;
                result.Errors.Add(new BatchError
                {
                    Index = i,
                    Identifier = $"ID {id}",
                    Error = "Product not found"
                });
            }
            else
            {
                toDeleteIds.Add(i);
                result.SuccessCount++;
            }
        }
        if (toDeleteIds.Any())
        {
            var productsToDelete = existingProducts.Where(p=> toDeleteIds.Contains(p.Id)).ToList();
            result.ProcessedProducts = productsToDelete.Select(MapToResponce).ToList();

            await _repository.DeleteRangeProductsAsync(toDeleteIds);
            await _repository.SaveChangesAsync();
        }

        return result;

    }

    public async Task<BatchOperationResult> UpdeateBatchAsync(List<UpdateProductRequest> request)
    {
        var result = new BatchOperationResult
        {
            TotalReceive = request.Count,
            ProcessedProducts = new List<ProductResponce>(),
            Errors = new List<BatchError>()
        };

        if (request is null || request.Count == 0)
            return result;
        var ids = request.Select(r => r.Id).ToList();
        var existingProducts = await _repository.GetByIdsAsync(ids);
#pragma warning disable CS8604 // Possible null reference argument.
        var existingDict = existingProducts.ToDictionary(p => p.Id);
#pragma warning restore CS8604 // Possible null reference argument.
        var updateProducts = new List<Product>();
        for (int i=0; i < request.Count; i++)
        {
            var req = request[i];
            if (!existingDict.ContainsKey(req.Id))
            {
                result.FailCount++;
                result.Errors.Add(new BatchError
                {
                    Index = i,
                    Identifier = $"ID {req.Id}",
                    Error = "Product not found"
                });
                continue;
            }
            try
            {
                var product = existingDict[req.Id];
                if (!string.IsNullOrWhiteSpace(req.Name))
                    product.UpdateName(req.Name);
                if (req.Price.HasValue)
                    product.UpdatePrice(req.Price.Value);
                if (req.Stock.HasValue)
                {
                    var diff = req.Stock.Value - product.Stock;
                    if (diff>0)
                        product.AddStock(diff);
                    else if(diff<0)
                        product.RemoveStock(diff);
                }
                updateProducts.Add(product);
                result.SuccessCount++;
            }
            catch(Exception ex)
            {
                result.FailCount++;
                result.Errors.Add(new BatchError
                {
                    Index = i,
                    Identifier = req.Name ?? $"ID{req.Id}",
                    Error = ex.Message
                });
            }
        }
        if (updateProducts.Any())
        {
            await _repository.UpdateRangeAsync(updateProducts);
            await _repository.SaveChangesAsync();
        }
        result.ProcessedProducts = updateProducts.Select(MapToResponce).ToList();
        return result;
    }

    private static ProductResponce MapToResponce(Product product)
    {
        return new ProductResponce
        {
            Id=product.Id,
            Name=product.Name,
            Stock = product.Stock,
            Price = product.Price
        };
    }
}


