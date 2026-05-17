

namespace StockMaster.Domain.Entities;

public class Product
{
    public int Id { get; private set;}
    public string Name { get; private set;} = string.Empty;
    public int Stock { get; private set;}
    public decimal Price { get; private set;}

    private Product(){}

    public Product(int id, string name, int stock, decimal price)
    {
        Id = id;
        Name = name;
        Stock = stock;
        Price = price;
    }

        public Product(string name, int stock, decimal price)
    {
        if (name ==null) 
            throw new ArgumentNullException("Имя не должно быть пустым");

        if (stock < 0)
            throw new ArgumentException("Остаток не может быть отрицательным");
            
        if (price <= 0)
            throw new ArgumentException("Цена должна быть положительной");
        Name = name;
        Stock = stock;
        Price = price;
    }

    

    public bool IsInStock => Stock > 0;
    public string DisplayName => $"{Name} (${Price})";

    public void AddStock(int quantity)
    {
        if (quantity <=0)
            throw new ArithmeticException("Количество должно быть положительным");
        
        Stock += quantity;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <=0) 
            throw new ArithmeticException("Количество должно быть положительным");

        if (quantity > Stock)
            throw new ArithmeticException($"Недостаточно товара. Остаток:{Stock}");

        Stock -= quantity;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Название не может быть пустым");
        Name = newName;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice<=0)
            throw new ArgumentException("Цена должна быть положительной");
        Price = newPrice;
    }



    public override string ToString()
    {
        return $"Product #{Id}: {Name}, Stock: {Stock}, Price: {Price:C}";
    }


}





