namespace Gudena.Data.Entities;

public class Favourite
{
    public int Id { get; set; }
    public DateTime FavouriteDate { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }

    public User User { get; set; }
    public Product Product { get; set; }
}