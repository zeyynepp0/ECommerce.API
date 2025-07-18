// Favorite entity'sini içeren namespace'i içeri aktarır
using ECommerce.API.Entities.Concrete;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // Favorite entity'si için repository arayüzü. IRepository<Favorite> arayüzünden türetilir.
    public interface IFavoriteRepository : IRepository<Favorite>
    {
        // Belirli bir kullanıcıya ait favori ürünleri asenkron olarak getirir
        Task<List<Favorite>> GetFavoritesByUserIdAsync(int userId);
        // Belirli bir kullanıcı ve ürün için favorilere ekleme işlemini asenkron olarak yapar
        Task<bool> AddToFavoritesAsync(int userId, int productId);
        // Belirli bir kullanıcı ve ürün için favorilerden çıkarma işlemini asenkron olarak yapar
        Task<bool> RemoveFromFavoritesAsync(int userId, int productId);
        // Bir ürünün kullanıcı tarafından favorilere eklenip eklenmediğini asenkron olarak kontrol eder
        Task<bool> IsProductFavoritedByUserAsync(int userId, int productId);
        IQueryable<Favorite> Favorites { get; }
        Task<Favorite> GetFavoriteByUserAndProductAsync(int userId, int productId);
    }
} 