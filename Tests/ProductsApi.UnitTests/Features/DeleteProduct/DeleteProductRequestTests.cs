using Castle.Core.Internal;

using RefactorThis.Attributes;
using RefactorThis.Features.DeleteProduct;

using Xunit;

namespace RefactorThis.UnitTests.Features.DeleteProduct;

public class DeleteProductRequestTests
{
    [Fact]
    public void HasTransactionScopeAttribute()
    {
        Assert.NotNull(typeof(DeleteProductRequest).GetAttribute<TransactionScopeAttribute>());
    }
}