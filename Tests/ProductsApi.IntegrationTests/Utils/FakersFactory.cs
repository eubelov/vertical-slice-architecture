using Bogus;

using ProductsApi.DataAccess.Entities;

namespace ProductsApi.IntegrationTests.Utils;

public static class FakersFactory
{
    public static Faker<ProductOption> GetProductOptionFaker(Guid productId)
    {
        var faker = new Faker<ProductOption>();
        faker
            .RuleFor(x => x.Id, x => x.Random.Guid())
            .RuleFor(x => x.ProductId, _ => productId)
            .RuleFor(x => x.Name, x => x.Random.String2(1, 9))
            .RuleFor(x => x.Description, x => x.Random.String2(1, 23));

        return faker;
    }
}