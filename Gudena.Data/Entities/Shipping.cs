namespace Gudena.Data.Entities;

public class Shipping
{
    public int Id { get; set; }
    public string ShippingAddress { get; set; }
    public string DeliveryOption { get; set; }
    public string ShippingNumbers { get; set; }
    public decimal ShippingCost { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<WarrantyClaim> WarrantyClaims { get; set; }
    public ICollection<ProductReturn> ProductReturns { get; set; }
}