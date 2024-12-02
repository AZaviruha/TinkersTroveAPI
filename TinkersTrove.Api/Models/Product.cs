namespace TinkersTrove.Api.Models;

public class Product
{
    public int Id { get; init; }

    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public int StockQuantity { get; set; }
    
    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public ICollection<Price> Prices { get; set; } = new List<Price>();

    public decimal CurrentPrice => Prices.MaxBy(p => p.EffectiveDate)?.Amount ?? 0m;
}