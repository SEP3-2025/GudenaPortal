namespace Gudena.Data.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public string PaymentMethod { get; set; }
    public decimal TotalAmount { get; set; }
    public int UserId { get; set; }
    public int ShippingId { get; set; }

    public User User { get; set; }
    public Shipping Shipping { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<Payment> Payments { get; set; }
}