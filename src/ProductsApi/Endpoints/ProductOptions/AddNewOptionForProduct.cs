using Microsoft.AspNetCore.Mvc;

using ProductsApi.Features.AddNewOptionForProduct;

namespace ProductsApi.Endpoints.ProductOptions;

public class AddNewOptionForProduct : EndpointBase
{
    /// <summary>
    /// Adds a new product option to the specified product.
    /// </summary>
    /// <response code="201">Product option added.</response>
    /// <response code="400">Some of the request's parameters have invalid format.</response>
    /// <response code="404">Product with the specified ID does not exist.</response>
    /// <response code="500">Unexpected error occured. Examine the response for details.</response>
    [HttpPost("products/{productId:guid}/options", Name = "AddNewOptionForProduct")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddNewOptionForProductResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [ApiExplorerSettings(GroupName = "Product Options")]
    public async Task<IActionResult> Execute(
        [FromRoute] Guid productId,
        [FromBody] AddNewOptionForProductRequest.OptionModel optionModel,
        CancellationToken cancellationToken)
    {
        return await this.Send(
                   new AddNewOptionForProductRequest { ProductId = productId, Option = optionModel },
                   option => this.Created($"/api/v1/products/{productId}/options/{option.OptionId}", option),
                   cancellationToken);
    }
}