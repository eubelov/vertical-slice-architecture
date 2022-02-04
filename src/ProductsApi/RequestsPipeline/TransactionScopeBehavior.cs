using System.Reflection;

using MediatR;

using RefactorThis.Attributes;
using RefactorThis.DataAccess;

namespace RefactorThis.RequestsPipeline;

public sealed class TransactionScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Context context;

    public TransactionScopeBehavior(Context context)
    {
        this.context = context;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        TResponse response;
        var attribute = typeof(TRequest).GetCustomAttribute<TransactionScopeAttribute>();
        if (attribute == null || this.context.Database.CurrentTransaction != null)
        {
            response = await next();
        }
        else
        {
            await using var transaction = this.context.Database.BeginTransaction();
            response = await next();
            await transaction.CommitAsync(cancellationToken);
        }

        return response;
    }
}