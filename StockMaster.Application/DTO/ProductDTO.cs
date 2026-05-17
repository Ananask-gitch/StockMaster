namespace StockMaster.Application.DTOs;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public int Stock {get; set;}
    public decimal Price { get; set;}

}

public class UpdateProductRequest
{

    public int Id {get; set;}
    public string? Name { get; set; } 
    public int? Stock {get; set;}
    public decimal? Price { get; set;}
}

public class ProductResponce
{
    public int Id { get; set;}
    public string Name { get; set;} = string.Empty;
    public int Stock{get; set;}
    public decimal Price { get; set;}
}

public class BatchOperationResult
{
    public int TotalReceive {get; set;}
    public int SuccessCount {get; set;}
    public int FailCount {get; set;}
    public List<ProductResponce> ProcessedProducts {get;set;}=new();
    public List<BatchError> Errors{get; set;}=new();
}

public class BatchError
{
    public int? Index {get; set;}
    public string? Identifier{get; set;}
    public string Error {get; set;} = string.Empty;
    public int? ProvidedStock {get; set;}
    public decimal? ProvidedPrice {get; set;}
}