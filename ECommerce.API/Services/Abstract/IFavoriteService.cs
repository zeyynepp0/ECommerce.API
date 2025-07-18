// Favori servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Favori varlık sınıfı
using ECommerce.API.DTO; // DTO sınıfları

namespace ECommerce.API.Services.Abstract
{
    // Favori işlemleri için servis arayüzü
    public interface IFavoriteService
    {
        Task<List<FavoriteDto>> GetFavoritesByUserIdAsync(int userId); // Kullanıcıya ait favori ürünleri getirir (asenkron)
        Task<(bool success, string message)> AddToFavoritesAsync(int userId, int productId); // Favorilere ürün ekler (asenkron)
        Task<bool> RemoveFromFavoritesAsync(int userId, int productId); // Favorilerden ürün kaldırır (asenkron)
        Task<bool> IsProductFavoritedByUserAsync(int userId, int productId); // Ürünün favori olup olmadığını kontrol eder (asenkron)
    }
} 