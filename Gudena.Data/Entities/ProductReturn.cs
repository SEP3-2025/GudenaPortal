namespace Gudena.Data.Entities;

public class ProductReturn
{
    public int Id { get; set; }
    public DateTime RequestDate { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
    public int ProductId { get; set; }
    public int OrderItemId { get; set; }

    public Product Product { get; set; } = null!;
    public OrderItem OrderItem { get; set; } = null!;
}