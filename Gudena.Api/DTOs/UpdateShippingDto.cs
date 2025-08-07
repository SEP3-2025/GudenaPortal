namespace Gudena.Api.DTOs;

public class UpdateShippingDto
{
        
    public string City { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string DeliveryOption { get; set; }
    public string ShippingNumbers { get; set; }
    public string ShippingStatus { get; set; }

    // Many-to-many relation
    public List<int> OrderItemIds { get; set; } = new List<int>();
}