using MediatR;

using ProductsApi.Models;

namespace ProductsApi.Features.Login;

public sealed class LoginRequest : IRequest<MediatorResponse<LoginResponse>>
{
    public string UserName { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}