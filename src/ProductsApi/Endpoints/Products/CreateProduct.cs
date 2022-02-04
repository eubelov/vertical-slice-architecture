using Microsoft.AspNetCore.Mvc;

using ProductsApi.Features.CreateProduct;

namespace ProductsApi.Endpoints.Products;

public class CreateProduct : EndpointBase
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <returns>Details of the created product.</returns>
    /// <response code="201">Product created.</response>
    /// <response code="400">Some of the request's parameters have invalid format.</response>
    /// <response code="500">Unexpected error occured. Examine the response for details.</response>
    [HttpPost("products", Name = "CreateProduct")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateProductResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [ApiExplorerSettings(GroupName = "Products")]
    public async Task<IActionResult> Execute([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        return await this.Send(request, product => this.Created($"/api/v1/products/{product.Id}", product), cancellationToken);
    }
}