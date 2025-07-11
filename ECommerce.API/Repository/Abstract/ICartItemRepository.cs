using ECommerce.API.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repository.Abstract
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        DbSet<CartItem> CartItems { get; }

        Task<List<CartItem>> GetAllWithIncludesAsync();
        Task<CartItem> GetByIdWithIncludesAsync(int id);
        Task<CartItem> GetCartItemByUserAndProductAsync(int userId, int productId);
        Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId);
        Task DeleteRangeAsync(List<CartItem> items);
        Task SaveChangesAsync();

    }
}