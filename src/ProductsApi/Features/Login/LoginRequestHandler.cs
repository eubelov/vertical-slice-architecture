using MediatR;

using ProductsApi.DataAccess.Entities;
using ProductsApi.DataAccess.EntityService;
using ProductsApi.Metrics;
using ProductsApi.Models;
using ProductsApi.Providers;

namespace ProductsApi.Features.Login;

public sealed class LoginRequestHandler : IRequestHandler<LoginRequest, MediatorResponse<LoginResponse>>
{
    private readonly IReadOnlyEntityService entityService;

    private readonly IDateTimeProvider dateTimeProvider;

    private readonly ILogger<LoginRequestHandler> logger;

    public LoginRequestHandler(IReadOnlyEntityService entityService, IDateTimeProvider dateTimeProvider, ILogger<LoginRequestHandler> logger)
    {
        this.entityService = entityService;
        this.dateTimeProvider = dateTimeProvider;
        this.logger = logger;
    }

    public async Task<MediatorResponse<LoginResponse>> Handle(LoginRequest request, CancellationToken token)
    {
        var spec = new FindUserByCredentialsSpec(request.UserName, request.Password);
        var userToken = await this.entityService.SingleOrDefault<User, FindUserByCredentialsSpec>(spec, token);
        if (userToken == null)
        {
            this.logger.LogInformation("User not found");
            ApiMetrics.FailedLoginAttemptsCount.Labels(nameof(LoginResponse.FailureReason.WrongUserNameOrPassword)).Inc();

            return new()
            {
                Result = LoginResponse.WrongUserNameOrPassword,
            };
        }

        if (this.dateTimeProvider.Now.ToUnixTimeMilliseconds() > userToken.ApiTokenExpiry)
        {
            ApiMetrics.FailedLoginAttemptsCount.Labels(nameof(LoginResponse.FailureReason.ExpiredToken)).Inc();

            return new()
            {
                Result = LoginResponse.TokenExpired,
            };
        }

        return new()
        {
            Result = new()
            {
                Token = userToken.ApiTokenGuid,
            },
        };
    }
}