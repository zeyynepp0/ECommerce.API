// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // Category entity'si için
using ECommerce.API.Repository.Abstract; // ICategoryRepository arayüzü için
using Microsoft.EntityFrameworkCore;

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Kategorilere özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        // CategoryRepository constructor
        // <param name="context">Veritabanı context'i</param>
        public CategoryRepository(MyDbContext context) : base(context)
        {
            // Ek bir işlem yapılmıyor, base constructor çağrılıyor
        }

        public async Task<int> CountAsync()
        {
            return await _context.Categories.CountAsync();
        }
    }
}
