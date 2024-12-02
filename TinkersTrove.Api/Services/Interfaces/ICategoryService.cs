using TinkersTrove.Api.DTOs;

namespace TinkersTrove.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int? parentId, bool shouldIncludeChildren = false);
    
    Task<CategoryDto?> GetCategoryByIdAsync(int id, bool shouldIncludeChildren = false);
    
    Task<CategoryDto?> GetCategoryByNameAsync(string name, bool shouldIncludeChildren = false);
    
    Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);
    
    Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryDto categoryDto);
    
    Task DeleteCategoryAsync(int id);
}

