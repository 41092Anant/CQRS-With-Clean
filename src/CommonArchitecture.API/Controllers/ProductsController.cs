using CommonArchitecture.Application.Commands.Products.CreateProduct;
using CommonArchitecture.Application.Commands.Products.DeleteProduct;
using CommonArchitecture.Application.Commands.Products.UpdateProduct;
using CommonArchitecture.Application.DTOs;
using CommonArchitecture.Application.Queries.Products.GetAllProducts;
using CommonArchitecture.Application.Queries.Products.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommonArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAll([FromQuery] ProductQueryParameters parameters)
    {
        var query = new GetAllProductsQuery(parameters);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _mediator.Send(query);
        
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto createDto)
    {
        var command = new CreateProductCommand(
            createDto.Name,
            createDto.Description,
            createDto.Price,
            createDto.Stock
        );
        
        var product = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdateProductDto updateDto)
    {
        var command = new UpdateProductCommand(
            id,
            updateDto.Name,
            updateDto.Description,
            updateDto.Price,
            updateDto.Stock
        );
        
        var result = await _mediator.Send(command);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteProductCommand(id);
        var result = await _mediator.Send(command);
        
        if (!result)
            return NotFound();

        return NoContent();
    }
}
