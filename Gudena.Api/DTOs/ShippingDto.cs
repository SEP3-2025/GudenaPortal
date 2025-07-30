namespace Gudena.Api.DTOs
{
    public class ShippingDto
    {
        public int Id { get; set; }
        public string ShippingAddress { get; set; }
        public string DeliveryOption { get; set; }
        public string ShippingNumbers { get; set; }
        public decimal ShippingCost { get; set; }
        public string ShippingStatus { get; set; }

        // Many-to-many relation
        public List<int> OrderItemIds { get; set; } = new List<int>();
    }
}
