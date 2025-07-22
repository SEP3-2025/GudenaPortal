namespace Gudena.Data.Entities;

public class Shipping
{
    public int Id { get; set; }
    public string ShippingAddress { get; set; } = null!;
    public string DeliveryOption { get; set; } = null!;
    public string ShippingNumbers { get; set; } = null!;
    public decimal ShippingCost { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
