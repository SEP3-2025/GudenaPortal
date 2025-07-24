namespace Gudena.Api.Exceptions;

public class ProductAlreadyReturnedException : Exception
{
    public ProductAlreadyReturnedException()
    {
        
    }

    public ProductAlreadyReturnedException(string message)
        : base(message)
    {
        
    }

    public ProductAlreadyReturnedException(string message, Exception inner)
        : base(message, inner)
    {
        
    }
}