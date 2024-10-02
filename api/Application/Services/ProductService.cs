using System.Collections.Generic;
using api.Contracts.Requests;
using api.Contracts.Responses;
using api.Domain.Entities;
using api.Infrastructure.Repositories;
using api.Presentation.Mappings;

namespace api.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _productRepository.GetAll();
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product?.ToResponseDto();
    }

    public async Task<ProductResponseDto> AddProductAsync(ProductRequestDto productRequest)
    {
        var product = Product.Create(productRequest.Name, productRequest.Description, productRequest.Price, productRequest.VATPercentage, productRequest.CompanyId);
        await _productRepository.AddAsync(product);
        return product.ToResponseDto();
    }

    public async Task<ProductResponseDto> UpdateProductAsync(int id, ProductRequestDto productRequest)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        product.Update(productRequest.Name, productRequest.Description, productRequest.Price, productRequest.VATPercentage);
        await _productRepository.UpdateAsync(product);
        return product.ToResponseDto();
    }

    public async Task DeleteProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            _productRepository.Delete(product);
        }
    }
}
