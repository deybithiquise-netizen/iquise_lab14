using iquise_lab14.Api.Models;
using iquise_lab14.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace iquise_lab14.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(ProductStore store) : ControllerBase
{
    /// <summary>Lists all products.</summary>
    [HttpGet]
    public ActionResult<IReadOnlyList<Product>> GetAll() => Ok(store.GetAll());

    /// <summary>Gets a product by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product> GetById(int id) =>
        store.GetById(id) is { } product ? Ok(product) : NotFound();

    /// <summary>Creates a product.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Product> Create(CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Price <= 0)
        {
            return BadRequest("Name is required and price must be greater than zero.");
        }

        var product = store.Add(request);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>Deletes a product.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id) => store.Delete(id) ? NoContent() : NotFound();
}
