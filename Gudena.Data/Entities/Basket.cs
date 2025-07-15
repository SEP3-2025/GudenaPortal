namespace Gudena.Data.Entities;

public class Basket
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public ICollection<Product> Products { get; set; }

    public User User { get; set; }
}