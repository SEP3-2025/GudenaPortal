namespace Gudena.Api.Exceptions;

public class UserDoesNotOwnResourceException : Exception
{
    public UserDoesNotOwnResourceException()
    {
        
    }

    public UserDoesNotOwnResourceException(string message)
        : base(message)
    {
        
    }

    public UserDoesNotOwnResourceException(string message, Exception inner)
    : base(message, inner)
    {
        
    }
}