namespace Gudena.Api.Exceptions;

public class IncancellableOrderException : Exception
{
    public IncancellableOrderException()
    {
        
    }

    public IncancellableOrderException(string message)
        : base(message)
    {
        
    }

    public IncancellableOrderException(string message, Exception inner)
        : base(message, inner)
    {
        
    }
}