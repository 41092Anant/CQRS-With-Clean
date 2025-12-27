using CommonArchitecture.Application.DTOs;
using CommonArchitecture.Application.Queries.Products.GetAllProducts;
using CommonArchitecture.Core.Entities;
using CommonArchitecture.Core.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CommonArchitecture.UnitTests.Products;

public class GetAllProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly GetAllProductsQueryHandler _handler;

    public GetAllProductsQueryHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new GetAllProductsQueryHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedProducts()
    {
        // Arrange
        var parameters = new ProductQueryParameters
        {
            PageNumber = 1,
            PageSize = 10,
            SearchTerm = "",
            SortBy = "Id",
            SortOrder = "asc"
        };
        var query = new GetAllProductsQuery(parameters);

        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 100, Stock = 10 },
            new Product { Id = 2, Name = "Product 2", Price = 200, Stock = 20 }
        };

        _productRepositoryMock.Setup(x => x.GetPagedAsync(
            parameters.SearchTerm,
            parameters.SortBy,
            parameters.SortOrder,
            parameters.PageNumber,
            parameters.PageSize))
            .ReturnsAsync(products);

        _productRepositoryMock.Setup(x => x.GetTotalCountAsync(parameters.SearchTerm))
            .ReturnsAsync(2);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        
        var items = result.Items.ToList();
        items[0].Name.Should().Be("Product 1");
        items[1].Name.Should().Be("Product 2");
    }
}
