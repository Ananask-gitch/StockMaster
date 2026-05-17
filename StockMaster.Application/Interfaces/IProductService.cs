
using StockMaster.Application.DTOs;
using StockMaster.Application.Service;

namespace StockMaster.Application.Interfaces;

public interface IProductService
{

    Task <List<ProductResponce>> GetAllAsync ();

    Task<List<ProductResponce>> GetByIdsAsync (List<int> ids);

    Task<BatchOperationResult> CreateBatchAsync(List<CreateProductRequest> request);

    Task<BatchOperationResult> UpdeateBatchAsync(List<UpdateProductRequest> request);

    Task<BatchOperationResult> DeleteBatchAsync(List<int> ids);
}
