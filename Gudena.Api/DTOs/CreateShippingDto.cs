namespace Gudena.Api.DTOs
{
    public class CreateShippingDto
    {
        
        public string DeliveryOption { get; set; }
        public string ShippingNumbers { get; set; }
        public string ShippingStatus { get; set; }

        // Many-to-many relation
        public List<int> OrderItemIds { get; set; } = new List<int>();
    }
}
