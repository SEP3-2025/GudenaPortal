using System.Text.Json.Serialization;

namespace Gudena.Data.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Thumbnail { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Unit { get; set; }
    public bool IsDeleted { get; set; }
    public string ReferenceUrl { get; set; }
    public bool isReturnable { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public string OwnerId { get; set; }
    [JsonIgnore]
    public ApplicationUser Owner { get; set; }

    public ICollection<Media> Media { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<Favourite> Favourites { get; set; }
    public ICollection<WarrantyClaim> WarrantyClaims { get; set; }
    public ICollection<ProductReturn> ProductReturns { get; set; }
}