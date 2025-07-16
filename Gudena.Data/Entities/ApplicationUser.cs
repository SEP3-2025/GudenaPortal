namespace Gudena.Data.Entities;

using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string UserType { get; set; } = "Buyer";

    // Relation to AccountDetails
    public int? AccountDetailsId { get; set; }
    public AccountDetails AccountDetails { get; set; }

    public ICollection<Basket> Baskets { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Favourite> Favourites { get; set; }
    public ICollection<Product> Products { get; set; }
}
