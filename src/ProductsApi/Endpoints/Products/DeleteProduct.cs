using Microsoft.AspNetCore.Mvc;

using RefactorThis.Features.DeleteProduct;

namespace RefactorThis.Endpoints.Products;

public class DeleteProduct : EndpointBase
{
    /// <summary>
    /// Deletes a product and its options.
    /// </summary>
    /// <response code="204">Product and its options deleted.</response>
    /// <response code="400">Some of the request's parameters have invalid format.</response>
    /// <response code="404">Product with the specified ID does not exist.</response>
    /// <response code="500">Unexpected error occured. Examine the response for details.</response>
    [HttpDelete("products/{productId:guid}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [ApiExplorerSettings(GroupName = "Products")]
    public async Task<IActionResult> Execute([FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        return await this.Send(new DeleteProductRequest { ProductId = productId }, _ => this.NoContent(), cancellationToken);
    }
}