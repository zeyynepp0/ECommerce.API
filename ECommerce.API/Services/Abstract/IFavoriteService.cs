using ECommerce.API.Entities.Concrete;
using ECommerce.API.DTO;

namespace ECommerce.API.Services.Abstract
{
    public interface IFavoriteService
    {
       Task<List<FavoriteDto>> GetFavoritesByUserIdAsync(int userId);
        Task<(bool success, string message)> AddToFavoritesAsync(int userId, int productId);


        Task<bool> RemoveFromFavoritesAsync(int userId, int productId);
        Task<bool> IsProductFavoritedByUserAsync(int userId, int productId);
        IQueryable<Favorite> Favorites { get; }

    }
} 