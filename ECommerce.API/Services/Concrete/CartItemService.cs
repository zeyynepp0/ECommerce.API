using Microsoft.EntityFrameworkCore;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<List<CartItem>> GetAllAsync()
        {
            return await _cartItemRepository.GetAllWithIncludesAsync();
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            return await _cartItemRepository.GetByIdWithIncludesAsync(id);
        }

        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _cartItemRepository.GetCartItemsByUserIdAsync(userId);
        }

        public async Task AddAsync(CartItem cartItem)
        {
            var existingItem = await _cartItemRepository.GetCartItemByUserAndProductAsync(cartItem.UserId, cartItem.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
                _cartItemRepository.Update(existingItem);
            }
            else
            {
                await _cartItemRepository.AddAsync(cartItem);
            }

            await _cartItemRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(CartItem cartItem)
        {
            _cartItemRepository.Update(cartItem);
            await _cartItemRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _cartItemRepository.GetByIdAsync(id);
            if (item != null)
            {
                _cartItemRepository.Delete(item);
                await _cartItemRepository.SaveChangesAsync();
            }
        }

        public async Task ClearUserCartAsync(int userId)
        {
            var userCartItems = await _cartItemRepository.CartItems
        .Where(ci => ci.UserId == userId)
        .ToListAsync();

            await _cartItemRepository.DeleteRangeAsync(userCartItems);
        }
    }
}
