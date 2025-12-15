using MediatR;

namespace CommonArchitecture.Application.Commands.Products.DeleteProduct;

public record DeleteProductCommand(int Id) : IRequest<bool>;
