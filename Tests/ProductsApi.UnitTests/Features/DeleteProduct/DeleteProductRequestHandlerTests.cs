using RefactorThis.Features.DeleteProduct;

using Xunit;

namespace RefactorThis.UnitTests.Features.DeleteProduct;

public class DeleteProductRequestHandlerTests : UnitTestsBaseWithInMemoryContext
{
    private readonly DeleteProductRequestHandler handler;

    private readonly Guid productId = Guid.NewGuid();

    public DeleteProductRequestHandlerTests()
    {
        this.Context.Products.Add(new() { Id = this.productId });
        this.Context.ProductOptions.Add(new() { Id = Guid.NewGuid(), ProductId = this.productId });
        this.Context.ProductOptions.Add(new() { Id = Guid.NewGuid(), ProductId = this.productId });
        this.Context.SaveChanges();
        this.Context.ChangeTracker.Clear();

        this.UseNullLoggerFor<DeleteProductRequestHandler>();

        this.handler = this.Mocker.CreateInstance<DeleteProductRequestHandler>();
    }

    [Fact]
    public async Task CanDeleteProductAndItsOptions()
    {
        await this.handler.Handle(new() { ProductId = this.productId }, AnyToken);

        Assert.Empty(this.Context.Products);
        Assert.Empty(this.Context.ProductOptions);
    }
}