namespace Gudena.Api.Exceptions;


public class BusinessInvalidStatusChangeException : Exception
{
    public BusinessInvalidStatusChangeException(string message) : base(message)
    {
    }
}