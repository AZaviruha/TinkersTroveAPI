namespace TinkersTrove.Api.Models;

public class Price
{
    public int Id { get; init; }
    
    public decimal Amount { get; init; }
    
    public DateTime EffectiveDate { get; init; }
    
    public int ProductId { get; init; }

    public Product Product { get; init; } = null!;
}
