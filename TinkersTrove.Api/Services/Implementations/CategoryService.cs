using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TinkersTrove.Api.DAL;
using TinkersTrove.Api.DTOs;
using TinkersTrove.Api.Models;
using TinkersTrove.Api.Services.Interfaces;
using InvalidOperationException = System.InvalidOperationException;

namespace TinkersTrove.Api.Services.Implementations;

public class CategoryService(
    ApplicationDbContext context,
    IMapper mapper) : ICategoryService
{
    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int? parentId, bool shouldIncludeChildren)
    {
        var query = context.Categories.AsNoTracking();

        query = parentId.HasValue
            ? query.Where(c => c.ParentCategoryId == parentId)
            : query.Where(c => c.ParentCategoryId == null);

        if (shouldIncludeChildren)
        {
            query = query.Include(c => c.ChildCategories);
        }

        var categories = await query.ToListAsync();
        var categoryDtos = mapper.Map<IEnumerable<CategoryDto>>(categories);

        return categoryDtos;
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id, bool shouldIncludeChildren)
    {
        var query = context.Categories.AsQueryable();
            
        if (shouldIncludeChildren)
        {
            query = query.Include(c => c.ChildCategories);
        }

        var category = await query.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            return null;
        }

        var categoryDto = mapper.Map<CategoryDto>(category);
        return categoryDto;
    }
    
    // TODO: move common selector logic to BaseService.GetByQueryAsync
    public async Task<CategoryDto?> GetCategoryByNameAsync(string name, bool shouldIncludeChildren)
    {
        var query = context.Categories.AsQueryable();
            
        if (shouldIncludeChildren)
        {
            query = query.Include(c => c.ChildCategories);
        }

        var category = await query.FirstOrDefaultAsync(c => c.Name == name);
        if (category == null)
        {
            return null;
        }

        var categoryDto = mapper.Map<CategoryDto>(category);
        return categoryDto;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
    {
        var category = mapper.Map<Category>(categoryDto);

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var createdCategoryDto = mapper.Map<CategoryDto>(category);
        return createdCategoryDto;
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryDto categoryDto)
    {
        var existingCategory = await context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            return null;
        }

        existingCategory.Name = categoryDto.Name;
        existingCategory.ParentCategoryId = categoryDto.ParentCategoryId;

        await context.SaveChangesAsync();

        var updatedCategoryDto = mapper.Map<CategoryDto>(existingCategory);
        return updatedCategoryDto;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var existingCategory = await context.Categories
            .Include(c => c.ChildCategories)
            .FirstOrDefaultAsync();
        
        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Category with ID {id} not found");
        }

        if (existingCategory.ChildCategories.Count > 0)
        {
            throw new InvalidOperationException("Cannot delete a category that has child categories");
        }

        context.Categories.Remove(existingCategory);
        await context.SaveChangesAsync();
    }
}