using MediatR;

namespace ProductsApi.RequestsPipeline;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger logger;

    public LoggingBehavior(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger($"LoggingBehavior-{typeof(TRequest).Name}");
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        using var scope = this.logger.BeginScope(new KeyValuePair<string, object?>[] { new("requestName", typeof(TRequest).Name) });
        this.logger.LogInformation("Started request execution");
        return await next();
    }
}