namespace Gudena.Api.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public BasketDto Basket { get; set; }
    public string UserId { get; set; }
    public int ShippingId { get; set; }
    public int PaymentId { get; set; }
    public decimal Total { get; set; }
}