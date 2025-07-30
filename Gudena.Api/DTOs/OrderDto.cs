namespace Gudena.Api.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public List<OrderShippingDto> Shipping { get; set; }
    public int PaymentId { get; set; }
    public decimal Total { get; set; }
}