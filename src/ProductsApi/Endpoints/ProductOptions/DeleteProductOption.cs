using Microsoft.AspNetCore.Mvc;

using ProductsApi.Features.DeleteProductOption;

namespace ProductsApi.Endpoints.ProductOptions;

public sealed class DeleteProductOption : EndpointBase
{
    /// <summary>
    /// Deletes the specified product option.
    /// </summary>
    /// <response code="204">Product option deleted.</response>
    /// <response code="400">Some of the request's parameters have invalid format.</response>
    /// <response code="404">Product or product option with the specified ID does not exist. Examine the response for details.</response>
    /// <response code="500">Unexpected error occured. Examine the response for details.</response>
    [HttpDelete("products/{productId:guid}/options/{optionId:guid}", Name = "DeleteProductOption")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [ApiExplorerSettings(GroupName = "Product Options")]
    public async Task<IActionResult> Execute([FromRoute] Guid productId, [FromRoute] Guid optionId, CancellationToken cancellationToken)
    {
        var request = new DeleteProductOptionRequest
        {
            ProductId = productId,
            OptionId = optionId,
        };

        return await this.Send(request, _ => this.NoContent(), cancellationToken);
    }
}