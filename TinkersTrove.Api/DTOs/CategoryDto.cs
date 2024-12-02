namespace TinkersTrove.Api.DTOs;

public class CategoryDto
{
    public int Id { get; init; }

    public string Name { get; set; } = string.Empty;
    
    public int? ParentCategoryId { get; set; }

    public List<CategoryDto> ChildCategories { get; set; } = new ();
}
