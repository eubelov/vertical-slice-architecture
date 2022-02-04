namespace ProductsApi.Providers;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}