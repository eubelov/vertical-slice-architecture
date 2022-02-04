namespace RefactorThis.Providers;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now { get; } = DateTimeOffset.Now;
}