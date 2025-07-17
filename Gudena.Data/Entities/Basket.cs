using Gudena.Data.Entities;

public class Basket
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public ICollection<Product> Products { get; set; }
}