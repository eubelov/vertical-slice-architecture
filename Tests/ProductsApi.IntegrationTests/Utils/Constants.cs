namespace ProductsApi.IntegrationTests.Utils;

public class Constants
{
    public const string UserName = "user";

    public const string Password = "1234";

    public static readonly Guid UserToken = Guid.Parse("55A903CC-AFC8-4A86-854F-FBE85E7CCD71");

    public static class ProblemTypes
    {
        public const string Unauthorized = "https://refactor-this/api/unauthorized";

        public const string TokenExpired = "https://refactor-this/api/api-token-expired";

        public const string WrongCredentials = "https://refactor-this/api/wrong-credentials";

        public const string NotFound = "https://refactor-this/api/not-found";

        public const string ModelValidationError = "https://refactor-this/api/model-validation-error";
    }

    public static class Routes
    {
        public static class Auth
        {
            public const string Login = "/api/v1/auth/login";
        }

        public static class ProductOptions
        {
            public const string Delete = "/api/v1/products/{0}/options/{1}";

            public const string Create = "/api/v1/products/{0}/options";

            public const string GetForProduct = "/api/v1/products/{0}/options";
        }

        public static class Products
        {
            public const string Create = "/api/v1/products";

            public const string Delete = "/api/v1/products/{0}";

            public const string Update = "/api/v1/products/{0}";

            public const string GetById = "/api/v1/products/{0}";

            public const string Find = "/api/v1/products?name={0}";
        }
    }
}