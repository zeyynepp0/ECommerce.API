using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Repository.Abstract
{
    public interface IFavoriteRepository : IRepository<Favorite>
    {
        Task<List<Favorite>> GetFavoritesByUserIdAsync(int userId);
        Task<Favorite> GetFavoriteByUserAndProductAsync(int userId, int productId);
        Task<bool> IsProductFavoritedByUserAsync(int userId, int productId);
        IQueryable<Favorite> Favorites { get; }
    }
} 