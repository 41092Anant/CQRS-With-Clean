using CommonArchitecture.Application.DTOs;
using CommonArchitecture.Web.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CommonArchitecture.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IProductApiService _productApiService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;

    public ProductsController(
        IProductApiService productApiService, 
        ILogger<ProductsController> logger,
        IValidator<CreateProductDto> createValidator,
        IValidator<UpdateProductDto> updateValidator)
    {
        _productApiService = productApiService;
        _logger = logger;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // GET: Products
    public IActionResult Index()
    {
        return View();
    }

    // GET: Products/GetAll - AJAX endpoint
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ProductQueryParameters parameters)
    {
        try
        {
            var result = await _productApiService.GetAllAsync(parameters);
            return Json(new { success = true, data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching products");
            return Json(new { success = false, message = "Error fetching products" });
        }
    }

    // GET: Products/GetById/5 - AJAX endpoint
    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var product = await _productApiService.GetByIdAsync(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            return Json(new { success = true, data = product });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product {ProductId}", id);
            return Json(new { success = false, message = "Error fetching product" });
        }
    }

    // POST: Products/Create - AJAX endpoint
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] CreateProductDto createDto)
    {
        // Server-side validation with FluentValidation
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            return Json(new { success = false, errors = errors });
        }

        try
        {
            var product = await _productApiService.CreateAsync(createDto);
            return Json(new { success = true, message = "Product created successfully!", data = product });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return Json(new { success = false, message = "An error occurred while creating the product. Please try again." });
        }
    }

    // PUT: Products/Edit/5 - AJAX endpoint
    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [FromBody] UpdateProductDto updateDto)
    {
        // Server-side validation with FluentValidation
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            return Json(new { success = false, errors = errors });
        }

        try
        {
            var success = await _productApiService.UpdateAsync(id, updateDto);
            if (!success)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            return Json(new { success = true, message = "Product updated successfully!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {ProductId}", id);
            return Json(new { success = false, message = "An error occurred while updating the product. Please try again." });
        }
    }

    // DELETE: Products/Delete/5 - AJAX endpoint
    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _productApiService.DeleteAsync(id);
            if (!success)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            return Json(new { success = true, message = "Product deleted successfully!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product {ProductId}", id);
            return Json(new { success = false, message = "An error occurred while deleting the product. Please try again." });
        }
    }
}
