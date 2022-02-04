namespace ProductsApi.Exceptions;

public class BaseException : Exception
{
    public BaseException()
    {
    }

    public BaseException(string message)
        : base(message)
    {
    }

    public BaseException(string message, Exception ex)
        : base(message, ex)
    {
    }
}