namespace Gudena.Api.DTOs;

public class BusinessOrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public List<BusinessOrderItemDto> Items { get; set; } = new();
}