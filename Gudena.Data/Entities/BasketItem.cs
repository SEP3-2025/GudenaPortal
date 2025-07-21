namespace Gudena.Data.Entities;

public class BasketItem
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public int BasketId { get; set; }
    public Basket Basket { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}