namespace RefactorThis.Features.Login;

public sealed class LoginResponse
{
    public static readonly LoginResponse TokenExpired = new() { FailureReasonValue = FailureReason.ExpiredToken };

    public static readonly LoginResponse WrongUserNameOrPassword = new() { FailureReasonValue = FailureReason.WrongUserNameOrPassword };

    public enum FailureReason
    {
        WrongUserNameOrPassword,

        ExpiredToken,
    }

    public Guid? Token { get; init; }

    public FailureReason? FailureReasonValue { get; init; }
}