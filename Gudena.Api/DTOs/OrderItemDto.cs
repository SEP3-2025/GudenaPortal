namespace Gudena.Api.DTOs;

public class OrderItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public int ProductId { get; set; }
}