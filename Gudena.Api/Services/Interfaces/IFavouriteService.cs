using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public interface IFavouriteService
{
    public Task<List<Favourite>> GetFavouritesAsync(string userId);
    public Task<bool> IsFavouriteAsync(int productId, string userId);
    public Task<List<Favourite>> AddToFavouritesAsync(int productId, string userId);
    public Task<List<Favourite>> RemoveFromFavouritesAsync(int productId, string userId);
}