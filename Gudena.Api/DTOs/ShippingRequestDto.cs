using Gudena.Data.Entities;

public class ShippingRequestDto
{
    public int BasketId { get; set; }
    public string ShippingAddress { get; set; }
    public string DeliveryOption { get; set; } = "Standard"; // Default to Standard shipping
  
    public decimal ShippingCost { get; set; } // Default shipping cost
}