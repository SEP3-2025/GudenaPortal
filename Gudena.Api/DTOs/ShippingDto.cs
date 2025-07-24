namespace Gudena.Api.DTOs
{
    public class ShippingDto
    {
        public int Id { get; set; }
        public string ShippingAddress { get; set; }
        public string DeliveryOption { get; set; }
        public string ShippingNumbers { get; set; }
        public decimal ShippingCost { get; set; }
        public int? OrderItemId { get; set; } // Nullable to support dummy/unassigned logic
    }
}
