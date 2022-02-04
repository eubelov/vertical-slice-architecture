using Prometheus;

namespace RefactorThis.Metrics;

public static class ApiMetrics
{
    public const string Prefix = "refactor_this";

    public static readonly Counter FailedLoginAttemptsCount
        = Prometheus.Metrics.CreateCounter($"{Prefix}_failed_logins_count", "Number of failed login attempts", "reason");

    public static readonly Counter UnhandledErrorsCount
        = Prometheus.Metrics.CreateCounter($"{Prefix}_unhandled_errors_count", "Number of unhandled error", "exception_type");
}