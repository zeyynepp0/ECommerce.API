// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // Review entity'si için
using ECommerce.API.Repository.Abstract; // IReviewRepository arayüzü için
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Ürün yorumlarına özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        // Veritabanı context'i
        private readonly MyDbContext _context;

        // ReviewRepository constructor
        // <param name="context">Veritabanı context'i</param>
        public ReviewRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        // Belirli bir ürüne ait tüm yorumları getirir
        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }
    }
}
