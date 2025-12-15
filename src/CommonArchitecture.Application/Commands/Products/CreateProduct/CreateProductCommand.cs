using CommonArchitecture.Application.DTOs;
using MediatR;

namespace CommonArchitecture.Application.Commands.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock
) : IRequest<ProductDto>;
