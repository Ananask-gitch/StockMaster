using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using StockMaster.Application.DTOs;
using StockMaster.Application.Interfaces;

namespace StockMaster.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => 
        Ok(await _productService.GetAllAsync());

    [HttpGet("batch/get")]
    public async Task<IActionResult> GetById([FromBody] List<int> ids)
    {
        if (ids is null || ids.Count == 0)
          return BadRequest(new{error = "IDs лист неправильно"});

        var products = await _productService.GetByIdsAsync(ids);
        return Ok(products);
    }

    [HttpPost ("batch")]
    public async Task<IActionResult> CreateBatch([FromBody] List<CreateProductRequest> request) =>
        Ok(await _productService.CreateBatchAsync(request));
    [HttpPut ("batch")]
    public async Task<IActionResult> UpdateBatch([FromBody] List<UpdateProductRequest> request) =>
        Ok(await _productService.UpdeateBatchAsync(request));
    [HttpDelete ("batch")]
    public async Task<IActionResult> CreateBatch([FromBody] List<int> ids) =>
        Ok(await _productService.DeleteBatchAsync(ids));


    

}
