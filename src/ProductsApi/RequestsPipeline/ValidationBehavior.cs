using FluentValidation;

using MediatR;

using ProductsApi.Exceptions;
using ProductsApi.Models;

namespace ProductsApi.RequestsPipeline;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : MediatorResponse, new()
{
    private readonly IValidator<TRequest>[] validators;

    private readonly ILogger logger;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILoggerFactory loggerFactory)
    {
        this.validators = validators.ToArray();
        this.logger = loggerFactory.CreateLogger($"ValidationBehavior-{typeof(TRequest).Name}");
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        this.logger.LogInformation("Validating request");

        foreach (var validator in this.validators)
        {
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (result.IsValid)
            {
                continue;
            }

            var notFoundError = result.Errors.FirstOrDefault(x => x is EntityNotFoundValidationFailure);
            if (notFoundError != null)
            {
                return new()
                {
                    Exception = new EntityNotFoundException(notFoundError.ErrorMessage),
                };
            }

            var errors = result.Errors
                .Select(x => new { x.ErrorCode, x.ErrorMessage, x.PropertyName })
                .ToDictionary(x => x.PropertyName, x => (object?)x.ErrorMessage);

            return new()
            {
                Exception = new ModelValidationException(errors),
            };
        }

        this.logger.LogInformation("Request successfully validated");

        return await next();
    }
}