using ProductsApi.Features.DeleteProductOption;

using Xunit;

namespace ProductsApi.UnitTests.Features.DeleteProductOption;

public class DeleteProductOptionRequestHandlerTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid existingProductOptionId = Guid.NewGuid();

    private readonly Guid existingProductId = Guid.NewGuid();

    private readonly DeleteProductOptionRequestHandler handler;

    public DeleteProductOptionRequestHandlerTests()
    {
        this.Context.ProductOptions.Add(new() { Id = this.existingProductOptionId });
        this.Context.Products.Add(new() { Id = this.existingProductId });
        this.Context.SaveChanges();
        this.Context.ChangeTracker.Clear();

        this.UseNullLoggerFor<DeleteProductOptionRequestHandler>();
        this.handler = this.Mocker.CreateInstance<DeleteProductOptionRequestHandler>();
    }

    [Fact]
    public async Task CanDeleteProductOption()
    {
        await this.handler.Handle(new() { OptionId = this.existingProductOptionId, ProductId = this.existingProductId }, AnyToken);
        Assert.Empty(this.Context.ProductOptions);
    }
}