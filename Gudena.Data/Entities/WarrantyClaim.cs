namespace Gudena.Data.Entities;

public class WarrantyClaim
{
    public int Id { get; set; }
    public DateTime ClaimDate { get; set; }
    public string Status { get; set; }
    public string Reason { get; set; }
    public int ProductId { get; set; }
    public int OrderItemId { get; set; }

    public Product Product { get; set; }
    public OrderItem OrderItem { get; set; } 
}