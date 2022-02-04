namespace ProductsApi.Exceptions;

public class EntityNotFoundException : BaseException
{
    public EntityNotFoundException(string message)
        : base(message)
    {
    }
}