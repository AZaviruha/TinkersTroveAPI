using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TinkersTrove.Api.DAL;
using TinkersTrove.Api.DTOs;
using TinkersTrove.Api.Models;
using TinkersTrove.Api.Services.Interfaces;
using InvalidOperationException = System.InvalidOperationException;

namespace TinkersTrove.Api.Services.Implementations;

public class CategoryService : EntityService<Category>, ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public CategoryService(
        ApplicationDbContext context,
        IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int? parentId, bool shouldIncludeChildren)
    {
        var query = _context.Categories.AsNoTracking();

        query = parentId.HasValue
            ? query.Where(c => c.ParentCategoryId == parentId)
            : query.Where(c => c.ParentCategoryId == null);

        if (shouldIncludeChildren)
        {
            query = query.Include(c => c.ChildCategories);
        }
        
        var categories = await query.ToListAsync();
        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

        return categoryDtos;
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id, bool shouldIncludeChildren)
    {
        var query = _context.Categories.AsQueryable();
            
        if (shouldIncludeChildren)
        {
            query = query.Include(c => c.ChildCategories);
        }

        var category = await query.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            return null;
        }

        var categoryDto = _mapper.Map<CategoryDto>(category);
        return categoryDto;
    }
    
    // TODO: move common selector logic to BaseService.GetByQueryAsync
    public async Task<CategoryDto?> GetCategoryByNameAsync(string name, bool shouldIncludeChildren)
    {
        var query = _context.Categories.AsQueryable();
            
        if (shouldIncludeChildren)
        {
            query = query.Include(c => c.ChildCategories);
        }

        var category = await query.FirstOrDefaultAsync(c => c.Name == name);
        if (category == null)
        {
            return null;
        }

        var categoryDto = _mapper.Map<CategoryDto>(category);
        return categoryDto;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var createdCategoryDto = _mapper.Map<CategoryDto>(category);
        return createdCategoryDto;
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryDto categoryDto)
    {
        var existingCategory = await _context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            return null;
        }

        existingCategory.Name = categoryDto.Name;
        existingCategory.ParentCategoryId = categoryDto.ParentCategoryId;

        await _context.SaveChangesAsync();

        var updatedCategoryDto = _mapper.Map<CategoryDto>(existingCategory);
        return updatedCategoryDto;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var existingCategory = await _context.Categories
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

        _context.Categories.Remove(existingCategory);
        await _context.SaveChangesAsync();
    }
}