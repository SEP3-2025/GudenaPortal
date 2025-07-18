namespace Gudena.Api.DTOs;

public class OrderDto
{
    public Basket Basket { get; set; }
    public int UserId { get; set; }
    public int ShippingId { get; set; }
    public int PaymentId { get; set; }
    public double Total { get; set; }
}