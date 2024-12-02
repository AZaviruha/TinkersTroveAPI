using Microsoft.AspNetCore.Mvc;
using TinkersTrove.Api.DTOs;
using TinkersTrove.Api.Services.Interfaces;

namespace TinkersTrove.Api.Controllers;

[ApiController]
[Route("api/vi/[controller]")]
public class ProductsController(IProductService sProducts) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto? productDto)
    {
        if (productDto == null)
        {
            return BadRequest();
        }

        var existingProduct = await sProducts.GetProductByNameAsync(productDto.Name);
        if (existingProduct != null)
        {
            return BadRequest("Product with this name already exists");
        }

        var createProductDto = await sProducts.CreateProductAsync(productDto);
        return Ok(createProductDto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(int? categoryId)
    {
        var productDtos = await sProducts.GetProductsAsync(categoryId);
        return Ok(productDtos);
    }
}