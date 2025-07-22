namespace Gudena.Api.Exceptions;

public class OrderExceedsStockException : Exception
{
    public OrderExceedsStockException()
    {
        
    }

    public OrderExceedsStockException(string message)
        : base(message)
    {
        
    }

    public OrderExceedsStockException(string message, Exception inner)
        : base(message, inner)
    {
        
    }
}