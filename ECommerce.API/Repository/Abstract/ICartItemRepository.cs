// CartItem entity'sini içeren namespace'i içeri aktarır
using ECommerce.API.Entities.Concrete;
// Entity Framework işlemleri için gerekli namespace'i içeri aktarır
using Microsoft.EntityFrameworkCore;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // CartItem entity'si için repository arayüzü. IRepository<CartItem> arayüzünden türetilir.
    public interface ICartItemRepository : IRepository<CartItem>
    {
        // Belirli bir kullanıcıya ait sepet ürünlerini asenkron olarak getirir
        Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId);
        // Belirli bir kullanıcının sepetini asenkron olarak temizler
        Task ClearUserCartAsync(int userId);
        Task<List<CartItem>> GetAllWithIncludesAsync();
        Task<CartItem> GetByIdWithIncludesAsync(int id);
        Task<CartItem> GetCartItemByUserAndProductAsync(int userId, int productId);
        Task SaveChangesAsync();
        Task DeleteRangeAsync(List<CartItem> items);
        DbSet<CartItem> CartItems { get; }
    }
}