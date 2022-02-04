using Microsoft.AspNetCore.Mvc;

using RefactorThis.Features.FindProducts;

namespace RefactorThis.Endpoints.Products;

public class FindProducts : EndpointBase
{
    /// <summary>
    /// Finds all products matching the specified name.
    /// </summary>
    /// <response code="200">List of matching products.</response>
    /// <response code="400">Some of the request's parameters have invalid format.</response>
    /// <response code="500">Unexpected error occured. Examine the response for details.</response>
    [HttpGet("products", Name = "FindProducts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FindProductsResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [ApiExplorerSettings(GroupName = "Products")]
    public async Task<IActionResult> Execute([FromQuery] string? name, CancellationToken cancellationToken)
    {
        return await this.Send(new FindProductsRequest { Name = name }, this.Ok, cancellationToken);
    }
}