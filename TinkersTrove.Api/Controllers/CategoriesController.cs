using Microsoft.AspNetCore.Mvc;
using TinkersTrove.Api.DTOs;
using TinkersTrove.Api.Services.Interfaces;

namespace TinkersTrove.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController(ICategoryService sCategories) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto? categoryDto)
    {
        if (categoryDto == null)
        {
            return BadRequest(categoryDto);
        }

        var existingCategory = await sCategories.GetCategoryByNameAsync(categoryDto.Name);
        if (existingCategory != null)
        {
            return BadRequest("Category with this name already exists.");
        }

        var createdCategoryDto = await sCategories.CreateCategoryAsync(categoryDto);
        return CreatedAtRoute("GetCategoryById", new { id = createdCategoryDto.Id }, createdCategoryDto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(
        int? parentId = null,
        bool includeChildren = false)
    {
        var categories = await sCategories.GetCategoriesAsync(parentId, includeChildren);
        return Ok(categories);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto?>> GetCategoryById(
        int id,
        bool includeChildren = false)
    {
        var categoryDto = await sCategories.GetCategoryByIdAsync(id, includeChildren);
        if (categoryDto != null)
        {
            return Ok(categoryDto);
        }

        return NotFound();
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto?>> UpdateCategoryById(
        int id,
        [FromBody] CategoryDto? categoryDto)
    {
        if (categoryDto == null)
        {
            return BadRequest();
        }
        
        var updatedCategoryDto = await sCategories.UpdateCategoryAsync(id, categoryDto);
        if (updatedCategoryDto != null)
        {
            return Ok(updatedCategoryDto);
        }

        return NotFound();
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto?>> DeleteCategoryById(int id)
    {
        await sCategories.DeleteCategoryAsync(id);
        return Ok();
    }
}