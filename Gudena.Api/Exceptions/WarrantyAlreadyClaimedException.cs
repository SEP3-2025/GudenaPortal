namespace Gudena.Api.Exceptions;

public class WarrantyAlreadyClaimedException : Exception
{
    public WarrantyAlreadyClaimedException()
    {
        
    }

    public WarrantyAlreadyClaimedException(string message)
        : base(message)
    {
        
    }

    public WarrantyAlreadyClaimedException(string message, Exception inner)
        : base(message, inner)
    {
        
    }
}