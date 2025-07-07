using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Services.Abstract
{
    public interface ICartItemService
    {
        Task<List<CartItem>> GetAllAsync();
        Task<CartItem> GetByIdAsync(int id);
        Task AddAsync(CartItem cartItem);
        Task UpdateAsync(CartItem cartItem);
        Task DeleteAsync(int id);
    }
}