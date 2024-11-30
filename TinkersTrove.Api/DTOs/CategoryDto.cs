namespace TinkersTrove.Api.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
    public List<CategoryDto> ChildCategories { get; set; }
}
