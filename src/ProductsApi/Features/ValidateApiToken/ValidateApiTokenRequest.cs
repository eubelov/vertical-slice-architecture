using MediatR;

using RefactorThis.Models;

namespace RefactorThis.Features.ValidateApiToken;

public sealed class ValidateApiTokenRequest : IRequest<MediatorResponse<bool>>
{
    public Guid ApiToken { get; set; }
}