namespace Gudena.Data.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public string Status { get; set; } = "Pending";
    public Order Order { get; set; }
    public Product Product { get; set; }
    
    public ICollection<Shipping> Shippings { get; set; }
    public WarrantyClaim? WarrantyClaim { get; set; }
    public ProductReturn? ProductReturn { get; set; }
}