using TinkersTrove.Api.DTOs;

namespace TinkersTrove.Api.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(int? categoryId);
    
    Task<ProductDto?> GetProductByIdAsync(int id);
    
    Task<ProductDto?> GetProductByNameAsync(string name);
    
    Task<ProductDto> CreateProductAsync(ProductDto productDto);
    
    Task<ProductDto?> UpdateProductAsync(int id, ProductDto productDto);
    
    Task DeleteProductAsync(int id);
}