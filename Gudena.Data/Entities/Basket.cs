using System.Text.Json.Serialization;
using Gudena.Data.Entities;

public class Basket
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; }
    [JsonIgnore]
    public ApplicationUser ApplicationUser { get; set; }

    public ICollection<BasketItem> BasketItems { get; set; }
}