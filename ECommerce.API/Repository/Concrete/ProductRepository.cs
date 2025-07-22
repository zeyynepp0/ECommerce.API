// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // Product entity'si için
using ECommerce.API.Repository.Abstract; // IProductRepository arayüzü için
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Ürünlere özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        // Veritabanı context'i
        private readonly MyDbContext _context;

        // ProductRepository constructor
        // <param name="context">Veritabanı context'i</param>
        public ProductRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        // Kategorisiyle birlikte tüm ürünleri getirir
        public async Task<List<Product>> GetProductsWithCategoryAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        // Ürünleri isim veya açıklama anahtar kelimesine göre filtreler
        public async Task<List<Product>> GetFilteredAsync(string keyword)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
                .ToListAsync();
        }

        // Belirli bir kategoriye ait ve belirli bir ürünü hariç tutarak ürünleri getirir
        public async Task<List<Product>> GetByCategoryIdAsync(int categoryId, int excludeProductId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId && p.Id != excludeProductId)
                .ToListAsync();
        }

        // Toplam ürün sayısını döndürür
        public async Task<int> CountAsync()
        {
            return await _context.Products.CountAsync();
        }

        // Stok adedi belirli bir eşikten az olan ürünleri getirir
        public async Task<List<Product>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.Products.Where(p => p.StockQuantity < threshold).ToListAsync();
        }

        // Tüm ürünleri review'larıyla birlikte getirir
        public new async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Reviews).Include(p => p.Category).ToListAsync();
        }

        // Id'ye göre ürünü review'larıyla birlikte getirir
        public new async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Reviews).Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
