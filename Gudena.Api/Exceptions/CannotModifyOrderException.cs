namespace Gudena.Api.Exceptions;

public class CannotModifyOrderException : Exception
{
    public CannotModifyOrderException()
    {
        
    }

    public CannotModifyOrderException(string message)
        : base(message)
    {
        
    }

    public CannotModifyOrderException(string message, Exception inner)
        : base(message, inner)
    {
        
    }
}