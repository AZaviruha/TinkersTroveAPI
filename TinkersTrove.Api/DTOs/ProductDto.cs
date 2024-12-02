namespace TinkersTrove.Api.DTOs;

public class ProductDto
{
    public int Id { get; init; }

    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public decimal Price { get; set; }
    
    public int StockQuantity { get; set; }
    
    public int CategoryId { get; set; }
}
