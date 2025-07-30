namespace Gudena.Api.Exceptions;

public class UnpaidException : Exception
{
    public UnpaidException()
    {
        
    }

    public UnpaidException(string message)
        : base(message)
    {
        
    }

    public UnpaidException(string message, Exception inner)
        : base(message, inner)
    {
        
    }
}