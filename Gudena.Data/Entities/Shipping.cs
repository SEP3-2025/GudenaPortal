using System.Text.Json.Serialization;

namespace Gudena.Data.Entities;

public class Shipping
{
    public int Id { get; set; }
    
    public string City { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }

    public string DeliveryOption { get; set; }
    public string ShippingNumbers { get; set; }
    public decimal ShippingCost { get; set; }

    public string ShippingStatus { get; set; } = "Ordered";
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public int? OrderId { get; set; }
    public Order? Order { get; set; }

    public string ApplicationUserId { get; set; }
    [JsonIgnore]
    public ApplicationUser ApplicationUser { get; set; }
    
    public string BusinessUserId { get; set; }
    [JsonIgnore]
    public ApplicationUser BusinessUser { get; set; }
}

