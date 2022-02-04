using Microsoft.AspNetCore.Mvc;

using RefactorThis.Features.GetProductById;

namespace RefactorThis.Endpoints.Products;

public class GetProductById : EndpointBase
{
    /// <summary>
    /// Gets the product that matches the specified ID.
    /// </summary>
    /// <response code="200">Product's details.</response>
    /// <response code="400">Some of the request's parameters have invalid format.</response>
    /// <response code="404">Product with the specified ID does not exist.</response>
    /// <response code="500">Unexpected error occured. Examine the response for details.</response>
    [HttpGet("products/{productId:guid}", Name = "GetProductById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductByIdResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [ApiExplorerSettings(GroupName = "Products")]
    public async Task<IActionResult> Execute([FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        return await this.Send(new GetProductByIdRequest { ProductId = productId }, this.Ok, cancellationToken);
    }
}