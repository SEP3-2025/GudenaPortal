namespace Gudena.Data.Entities;

public class Favourite
{
    public int Id { get; set; }
    public DateTime FavouriteDate { get; set; }
    public string ApplicationUserId { get; set; }
    public int ProductId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
    public Product Product { get; set; }
}