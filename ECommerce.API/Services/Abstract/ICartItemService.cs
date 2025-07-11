using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Services.Abstract
{
    public interface ICartItemService
    {
        Task<List<CartItem>> GetAllAsync();
        Task<CartItem> GetByIdAsync(int id);
        Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId);
        Task AddAsync(CartItem cartItem);
        Task UpdateAsync(CartItem cartItem);
        Task DeleteAsync(int id);
        Task ClearUserCartAsync(int userId);
    }
}