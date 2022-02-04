using MediatR;

using ProductsApi.Models;

namespace ProductsApi.Features.ValidateApiToken;

public sealed class ValidateApiTokenRequest : IRequest<MediatorResponse<bool>>
{
    public Guid ApiToken { get; set; }
}