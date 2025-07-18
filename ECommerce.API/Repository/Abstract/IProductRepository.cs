// Product entity'sini içeren namespace'i içeri aktarır
using ECommerce.API.Entities.Concrete;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // Product entity'si için repository arayüzü. IRepository<Product> arayüzünden türetilir.
    public interface IProductRepository : IRepository<Product>
    {
        // Belirli bir kategoriye ait ve belirli bir ürünü hariç tutarak ürünleri asenkron olarak getirir
        Task<List<Product>> GetByCategoryIdAsync(int categoryId, int excludeProductId);
        Task<List<Product>> GetFilteredAsync(string keyword);
        Task<int> CountAsync();
        Task<List<Product>> GetLowStockProductsAsync(int threshold);
    }
}
