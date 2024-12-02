using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TinkersTrove.Api.DAL;
using TinkersTrove.Api.DTOs;
using TinkersTrove.Api.Models;
using TinkersTrove.Api.Services.Interfaces;

namespace TinkersTrove.Api.Services.Implementations;

public class ProductService : EntityService<Product>, IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(
        ApplicationDbContext context,
        IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(int? categoryId)
    {
        var query = _context.Products.AsNoTracking();

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }
        
        query = query.Include(p => p.Prices);
        
        var products = await query.ToListAsync();
        

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }


    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await GetAsync(p => p.Id == id);
        
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> GetProductByNameAsync(string name)
    {
        var product = await GetAsync(p => p.Name == name);
        
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        var price = new Price
        {
            Amount = productDto.Price,
            EffectiveDate = DateTime.UtcNow,
            ProductId = product.Id
        };
        
        product.Prices.Add(price);
        var createdProduct = await CreateAsync(product);
        
        return _mapper.Map<ProductDto>(createdProduct);
    }

    public async Task<ProductDto?> UpdateProductAsync(int id, ProductDto productDto)
    {
        var product = await GetAsync(p => p.Id == id);
        
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }
        
        product.Name = productDto.Name;
        product.Description = productDto.Description;
        product.StockQuantity = productDto.StockQuantity;
        product.CategoryId = productDto.CategoryId;

        if (product.CurrentPrice != productDto.Price)
        {
            var price = new Price
            {
                Amount = productDto.Price,
                EffectiveDate = DateTime.UtcNow,
                ProductId = product.Id
            };
            _context.Prices.Add(price);
        }

        await SaveAsync();
        
        return _mapper.Map<ProductDto>(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await GetAsync(p => p.Id == id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }
        
        await RemoveAsync(product);
    }
}