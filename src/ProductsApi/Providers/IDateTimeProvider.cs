namespace RefactorThis.Providers;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}