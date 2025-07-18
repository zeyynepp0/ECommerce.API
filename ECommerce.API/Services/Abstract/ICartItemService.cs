// Sepet servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Sepet varlık sınıfı

namespace ECommerce.API.Services.Abstract
{
    // Sepet işlemleri için servis arayüzü
    public interface ICartItemService
    {
        Task<List<CartItem>> GetAllAsync(); // Tüm sepet ürünlerini asenkron olarak getirir
        Task<CartItem> GetByIdAsync(int id); // Id'ye göre sepet ürününü asenkron olarak getirir
        Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId); // Kullanıcıya ait sepet ürünlerini getirir (asenkron)
        Task AddAsync(CartItem cartItem); // Yeni sepet ürünü ekler (asenkron)
        Task UpdateAsync(CartItem cartItem); // Sepet ürününü günceller (asenkron)
        Task DeleteAsync(int id); // Sepet ürününü siler (asenkron)
        Task ClearUserCartAsync(int userId); // Kullanıcının tüm sepetini temizler (asenkron)
    }
}