using MediatR;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Moq;
using Moq.AutoMock;

using ProductsApi.Providers;

namespace ProductsApi.UnitTests;

public class UnitTestBase
{
    public UnitTestBase()
    {
        this.Mediator = this.Mocker.GetMock<IMediator>();
        this.DateTimeProvider = this.Mocker.GetMock<IDateTimeProvider>();
        this.DateTimeProvider.SetupGet(x => x.Now).Returns(DateTime.Parse("2022-01-01T15:00:00"));
    }

    protected static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    protected AutoMocker Mocker { get; } = new();

    protected Mock<IMediator> Mediator { get; }

    protected Mock<IDateTimeProvider> DateTimeProvider { get; }

    protected void UseNullLoggerFor<T>()
    {
        this.Mocker.Use(
            typeof(ILogger<T>),
            NullLoggerFactory.Instance.CreateLogger<T>());
    }
}