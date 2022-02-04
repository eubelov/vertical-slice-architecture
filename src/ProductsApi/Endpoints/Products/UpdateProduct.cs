using Microsoft.AspNetCore.Mvc;

using ProductsApi.Features.UpdateProduct;

namespace ProductsApi.Endpoints.Products;

public class UpdateProduct : EndpointBase
{
    /// <summary>
    /// Updates a product.
    /// </summary>
    /// <response code="204">Product updated.</response>
    /// <response code="400">Some of the request's parameters have invalid format.</response>
    /// <response code="404">Product with the specified ID does not exist.</response>
    /// <response code="500">Unexpected error occured. Examine the response for details.</response>
    [HttpPut("products/{productId:guid}", Name = "UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [ApiExplorerSettings(GroupName = "Products")]
    public async Task<IActionResult> Execute(
        [FromRoute] Guid productId,
        [FromBody] ProductUpdateModel updateModel,
        CancellationToken cancellationToken)
    {
        var updateRequest = new UpdateProductRequest
        {
            Id = productId,
            Description = updateModel.Description,
            Name = updateModel.Name,
            Price = updateModel.Price,
            DeliveryPrice = updateModel.DeliveryPrice,
        };

        return await this.Send(updateRequest, _ => this.NoContent(), cancellationToken);
    }

    public class ProductUpdateModel
    {
        public string Name { get; init; } = string.Empty;

        public string? Description { get; init; }

        public decimal Price { get; init; }

        public decimal DeliveryPrice { get; init; }
    }
}