// Gerekli namespace'ler projeye dahil ediliyor
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // CartItem entity'si için
using ECommerce.API.Repository.Abstract; // ICartItemRepository arayüzü için
using System.Collections.Generic; // Koleksiyonlar için
using System.Linq; // LINQ işlemleri için
using System.Threading.Tasks; // Asenkron işlemler için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Sepet ürünlerine özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        // CartItemRepository constructor
        // <param name="context">Veritabanı context'i</param>
        public CartItemRepository(MyDbContext context) : base(context)
        {
        }

        // Tüm sepet ürünlerini kullanıcı ve ürün bilgileriyle birlikte getirir
        public async Task<List<CartItem>> GetAllWithIncludesAsync()
        {
            return await Context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.User)
                .ToListAsync();
        }

        // Id'ye göre sepet ürününü kullanıcı ve ürün bilgileriyle birlikte getirir
        public async Task<CartItem> GetByIdWithIncludesAsync(int id)
        {
            return await Context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.User)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        // Belirli bir kullanıcı ve ürün için sepet kaydını getirir
        public async Task<CartItem> GetCartItemByUserAndProductAsync(int userId, int productId)
        {
            return await Context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
        }

        // Belirli bir kullanıcıya ait tüm sepet ürünlerini getirir
        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await Context.CartItems
                .Where(ci => ci.UserId == userId)
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        // Belirli bir kullanıcının sepetini asenkron olarak temizler
        public async Task ClearUserCartAsync(int userId)
        {
            var items = await Context.CartItems.Where(ci => ci.UserId == userId).ToListAsync();
            if (items.Any())
            {
                Context.CartItems.RemoveRange(items);
                await Context.SaveChangesAsync();
            }
        }

        // Birden fazla sepet ürününü topluca siler
        public async Task DeleteRangeAsync(List<CartItem> items)
        {
            Context.CartItems.RemoveRange(items);
            await Context.SaveChangesAsync();
        }

        // Yapılan değişiklikleri veritabanına kaydeder (asenkron)
        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        // DbSet'e erişmek için kullanılabilir
        public DbSet<CartItem> CartItems => Context.CartItems;
    }
}

