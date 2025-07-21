namespace Gudena.Api.DTOs;

public class OrderUpdateDto
{
    public int Id { get; set; }
    public List<OrderItemDto> OrderItemUpdates { get; set; }
    public int ShippingId { get; set; }
    public int PaymentId { get; set; }
    public decimal Total { get; set; }
}