using Castle.Core.Internal;

using ProductsApi.Attributes;
using ProductsApi.Features.DeleteProduct;

using Xunit;

namespace ProductsApi.UnitTests.Features.DeleteProduct;

public class DeleteProductRequestTests
{
    [Fact]
    public void HasTransactionScopeAttribute()
    {
        Assert.NotNull(typeof(DeleteProductRequest).GetAttribute<TransactionScopeAttribute>());
    }
}