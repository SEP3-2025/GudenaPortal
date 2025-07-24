using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public class FavouriteService : IFavouriteService
{
    private readonly IFavouriteRepository _favouriteRepository;

    public FavouriteService(IFavouriteRepository favouriteRepository)
    {
        _favouriteRepository = favouriteRepository;
    }
    
    public async Task<List<Favourite>> GetFavouritesAsync(string userId)
    {
        return await _favouriteRepository.GetFavouritesAsync(userId);
    }

    public async Task<bool> IsFavouriteAsync(int productId, string userId)
    {
        return await _favouriteRepository.IsFavouriteAsync(productId, userId);
    }

    public async Task<List<Favourite>> AddToFavouritesAsync(int productId, string userId)
    {
        return await _favouriteRepository.AddToFavouritesAsync(productId, userId);
    }

    public async Task<List<Favourite>> RemoveFromFavouritesAsync(int productId, string userId)
    {
        return await _favouriteRepository.RemoveFromFavouritesAsync(productId, userId);
    }
}