using CommonArchitecture.Application.DTOs;
using MediatR;

namespace CommonArchitecture.Application.Queries.Products.GetAllProducts;

public record GetAllProductsQuery(ProductQueryParameters Parameters) : IRequest<PaginatedResult<ProductDto>>;
