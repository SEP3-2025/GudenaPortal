namespace Gudena.Api.Exceptions;

public class BasketExceedsStockException : Exception
{
    public BasketExceedsStockException()
    {
        
    }

    public BasketExceedsStockException(string message)
        : base(message)
    {
        
    }
    
    public BasketExceedsStockException(string message, Exception inner)
        : base(message, inner)
    {
        
    }
}