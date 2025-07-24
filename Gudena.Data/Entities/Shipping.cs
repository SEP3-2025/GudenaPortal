namespace Gudena.Data.Entities;

public class Shipping
{
    public int Id { get; set; }
    public string ShippingAddress { get; set; }
    public string DeliveryOption { get; set; }
    public string ShippingNumbers { get; set; }
    public decimal ShippingCost { get; set; }

    public string ShippingStatus { get; set; } = "Ordered"; // Added for tracking shipping status

    public int? OrderItemId { get; set; } // nullable for dummy/testing only; make required in final version
    public OrderItem OrderItem { get; set; }
}
