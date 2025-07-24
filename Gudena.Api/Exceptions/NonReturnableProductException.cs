namespace Gudena.Api.Exceptions;

public class NonReturnableProductException : Exception
{
    public NonReturnableProductException()
    {
        
    }

    public NonReturnableProductException(string message)
        : base(message)
    {
        
    }

    public NonReturnableProductException(string message, Exception inner)
        : base(message, inner)
    {
        
    }
}