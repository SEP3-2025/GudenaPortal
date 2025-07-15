namespace Gudena.Data.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserType { get; set; }

    public int AccountDetailsId { get; set; }
    public AccountDetails AccountDetails { get; set; }

    public ICollection<Basket> Baskets { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Favourite> Favourites { get; set; }
    public ICollection<Product> Products { get; set; }
}