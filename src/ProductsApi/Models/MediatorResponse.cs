namespace ProductsApi.Models;

public class MediatorResponse
{
    public Exception? Exception { get; init; }
}

public class MediatorResponse<T> : MediatorResponse
{
    public T? Result { get; init; }
}