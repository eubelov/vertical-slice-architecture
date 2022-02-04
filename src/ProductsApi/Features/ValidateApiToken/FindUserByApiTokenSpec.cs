using Ardalis.Specification;

using ProductsApi.DataAccess.Entities;

namespace ProductsApi.Features.ValidateApiToken;

public sealed class FindUserByApiTokenSpec : Specification<User>
{
    public FindUserByApiTokenSpec(Guid token, long unixTimeNow)
    {
        this.Query
            .AsNoTracking()
            .Where(x => x.ApiToken == token.ToString() && x.ApiTokenExpiry > unixTimeNow);
    }
}