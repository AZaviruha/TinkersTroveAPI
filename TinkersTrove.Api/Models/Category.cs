namespace TinkersTrove.Api.Models;

public class Category
{
    public int Id { get; init; }
    
    public string Name { get; set; } = string.Empty;
    
    public int? ParentCategoryId { get; set; }
    
    public Category ParentCategory { get; set; } = null!;

    public ICollection<Category> ChildCategories { get; set; } = new List<Category>();
}
