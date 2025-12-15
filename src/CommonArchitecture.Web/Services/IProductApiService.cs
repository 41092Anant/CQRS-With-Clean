using CommonArchitecture.Application.DTOs;

namespace CommonArchitecture.Web.Services;

public interface IProductApiService
{
    Task<PaginatedResult<ProductDto>> GetAllAsync(ProductQueryParameters parameters);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto createDto);
    Task<bool> UpdateAsync(int id, UpdateProductDto updateDto);
    Task<bool> DeleteAsync(int id);
}
