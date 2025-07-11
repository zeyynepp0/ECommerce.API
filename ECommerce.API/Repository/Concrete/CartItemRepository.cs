using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Repository.Concrete
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private readonly MyDbContext _context;

        public CartItemRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetAllWithIncludesAsync()
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.User)
                .ToListAsync();
        }

        public async Task<CartItem> GetByIdWithIncludesAsync(int id)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.User)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<CartItem> GetCartItemByUserAndProductAsync(int userId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
        }

        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        public async Task DeleteRangeAsync(List<CartItem> items)
        {
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Opsiyonel olarak DbSet'e erişmek istersen (kullanıyorsan):
        public DbSet<CartItem> CartItems => _context.CartItems;
    }

}

