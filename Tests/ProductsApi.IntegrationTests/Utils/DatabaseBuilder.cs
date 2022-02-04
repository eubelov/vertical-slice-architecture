using Microsoft.EntityFrameworkCore;

using RefactorThis.DataAccess;
using RefactorThis.DataAccess.Entities;

namespace RefactorThis.IntegrationTests.Utils;

public class DatabaseBuilder
{
    private readonly Context context;

    public DatabaseBuilder(Context context)
    {
        this.context = context;
    }

    public DatabaseStarter Initialize()
    {
        this.context.Database.EnsureDeleted();
        this.context.Database.Migrate();

        return new(this.context);
    }

    public class DatabaseStarter
    {
        private readonly Context context;

        public DatabaseStarter(Context context)
        {
            this.context = context;
        }

        public void Build()
        {
            this.context.SaveChanges();
        }

        public DatabaseStarter WithProduct(Product product)
        {
            this.context.Add(product);
            return this;
        }

        public DatabaseStarter WithProduct(Guid? productId = null, Action<Product>? modifier = null)
        {
            var product = new Product
            {
                Name = "test product",
                Description = "desc",
                Id = productId ?? Guid.NewGuid(),
                Price = 10.55M,
                DeliveryPrice = 6.33M,
            };
            modifier?.Invoke(product);
            this.context.Add(product);

            return this;
        }

        public DatabaseStarter WithProductOption(ProductOption productOption)
        {
            this.context.Add(productOption);
            return this;
        }

        public DatabaseStarter WithProductOption(Guid productId, Guid? optionId = null, Action<ProductOption>? modifier = null)
        {
            var product = new ProductOption
            {
                Name = "test product",
                Description = "desc",
                Id = optionId ?? Guid.NewGuid(),
                ProductId = productId,
            };
            modifier?.Invoke(product);
            this.context.Add(product);

            return this;
        }

        public DatabaseStarter WithUser(
            string userName = Constants.UserName,
            string password = Constants.Password,
            Action<User>? modifier = null)
        {
            var user = new User
            {
                Name = userName,
                Password = password,
                ApiToken = Constants.UserToken.ToString(),
                ApiTokenExpiry = DateTimeOffset.Now.AddYears(1).ToUnixTimeMilliseconds(),
            };
            modifier?.Invoke(user);
            this.context.Add(user);

            return this;
        }
    }
}