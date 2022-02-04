using MediatR;

using ProductsApi.Models;

namespace ProductsApi.RequestsPipeline;

public sealed class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : MediatorResponse, new()
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            return new()
            {
                Exception = ex,
            };
        }
    }
}