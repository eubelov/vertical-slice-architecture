namespace RefactorThis.Exceptions;

public class ModelValidationException : BaseException
{
    public ModelValidationException(Dictionary<string, object?> errors)
    {
        this.Errors = errors;
    }

    public Dictionary<string, object?> Errors { get; }
}