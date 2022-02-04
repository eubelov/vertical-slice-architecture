using Microsoft.AspNetCore.Mvc;

using RefactorThis.Features.FindOptionsForProduct;

namespace RefactorThis.Endpoints.ProductOptions;

public class FindOptionsForProduct : EndpointBase
{
    /// <summary>
    /// Finds all options for the specified product.
    /// </summary>
    /// <response code="200">List of options for the product.</response>
    /// <response code="400">Some of the request's parameters have invalid format.</response>
    /// <response code="404">Product with the specified ID does not exist.</response>
    /// <response code="500">Unexpected error occured. Examine the response for details.</response>
    [HttpGet("products/{productId:guid}/options", Name = "FindOptionsForProduct")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FindOptionsForProductResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [ApiExplorerSettings(GroupName = "Product Options")]
    public async Task<IActionResult> Execute([FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        return await this.Send(new FindOptionsForProductRequest { ProductId = productId }, this.Ok, cancellationToken);
    }
}