using MediatR;

using ProductsApi.DataAccess.EntityService;
using ProductsApi.Models;
using ProductsApi.Providers;

namespace ProductsApi.Features.ValidateApiToken;

internal sealed class ValidateApiTokenRequestHandler : IRequestHandler<ValidateApiTokenRequest, MediatorResponse<bool>>
{
    private readonly IReadOnlyEntityService entityService;

    private readonly IDateTimeProvider dateTimeProvider;

    public ValidateApiTokenRequestHandler(IReadOnlyEntityService entityService, IDateTimeProvider dateTimeProvider)
    {
        this.entityService = entityService;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<MediatorResponse<bool>> Handle(ValidateApiTokenRequest request, CancellationToken cancellationToken)
    {
        var unixTimeNow = this.dateTimeProvider.Now.ToUnixTimeMilliseconds();

        return new()
        {
            Result = await this.entityService.Exists(new FindUserByApiTokenSpec(request.ApiToken, unixTimeNow), cancellationToken),
        };
    }
}