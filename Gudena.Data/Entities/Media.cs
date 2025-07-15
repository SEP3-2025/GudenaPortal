namespace Gudena.Data.Entities;

public class Media
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string MediaUrl { get; set; }

    public Product Product { get; set; }
}