using Ardalis.Specification;

using RefactorThis.DataAccess.Entities;

namespace RefactorThis.Features.Login;

public sealed class FindUserByCredentialsSpec : Specification<User>, ISingleResultSpecification
{
    public FindUserByCredentialsSpec(string userName, string password)
    {
        this.Query
            .AsNoTracking()
            .Where(x => x.Name == userName && x.Password == password);
    }
}