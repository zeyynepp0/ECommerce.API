// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // Favorite entity'si için
using ECommerce.API.Repository.Abstract; // IFavoriteRepository arayüzü için
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Favorilere özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
    {
        // FavoriteRepository constructor
        // <param name="context">Veritabanı context'i</param>
        public FavoriteRepository(MyDbContext context) : base(context)
        {
        }

        // Tüm favori kayıtlarını sorgulamak için kullanılabilir
        public IQueryable<Favorite> Favorites => Context.Favorites;

        // Belirli bir kullanıcıya ait favori ürünleri getirir
        public async Task<List<Favorite>> GetFavoritesByUserIdAsync(int userId)
        {
            return await Context.Favorites
                .Include(f => f.Product)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        // Belirli bir kullanıcı ve ürün için favori kaydını getirir
        public async Task<Favorite> GetFavoriteByUserAndProductAsync(int userId, int productId)
        {
            return await Context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
        }

        // Bir ürünün kullanıcı tarafından favorilere eklenip eklenmediğini kontrol eder
        public async Task<bool> IsProductFavoritedByUserAsync(int userId, int productId)
        {
            return await Context.Favorites
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId);
        }

        // Belirli bir kullanıcı ve ürün için favorilere ekleme işlemini asenkron olarak yapar
        public async Task<bool> AddToFavoritesAsync(int userId, int productId)
        {
            if (await Context.Favorites.AnyAsync(f => f.UserId == userId && f.ProductId == productId))
                return false;
            var favorite = new Favorite { UserId = userId, ProductId = productId };
            await Context.Favorites.AddAsync(favorite);
            await Context.SaveChangesAsync();
            return true;
        }

        // Belirli bir kullanıcı ve ürün için favorilerden çıkarma işlemini asenkron olarak yapar
        public async Task<bool> RemoveFromFavoritesAsync(int userId, int productId)
        {
            var favorite = await Context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
            if (favorite == null)
                return false;
            Context.Favorites.Remove(favorite);
            await Context.SaveChangesAsync();
            return true;
        }
    }
} 