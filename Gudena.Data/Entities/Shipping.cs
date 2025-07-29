namespace Gudena.Data.Entities;

public class Shipping
{
    public int Id { get; set; }
    public string ShippingAddress { get; set; }
    public string DeliveryOption { get; set; }
    public string ShippingNumbers { get; set; }
    public decimal ShippingCost { get; set; }

    public string ShippingStatus { get; set; } = "Ordered";
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public int? OrderId { get; set; }
    public Order? Order { get; set; }
}
