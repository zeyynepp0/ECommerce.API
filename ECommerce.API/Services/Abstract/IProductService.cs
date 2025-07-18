// Ürün servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Ürün varlık sınıfı
using ECommerce.API.DTO; // DTO sınıfları

namespace ECommerce.API.Services.Abstract
{
    // Ürün işlemleri için servis arayüzü
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync(); // Tüm ürünleri asenkron olarak getirir
        Task<ProductDto> GetByIdAsync(int id); // Id'ye göre ürünü asenkron olarak getirir
        Task<List<Product>> GetByCategoryIdAsync(int categoryId, int excludeProductId); // Kategoriye göre ürünleri getirir, bir ürünü hariç tutar (asenkron)
        Task AddProductAsync(ProductDto dto); // Yeni ürün ekler (asenkron)
        Task UpdateProductAsync(int id, UpdateProductDto dto); // Ürünü günceller (asenkron)
        Task DeleteProductAsync(int id); // Ürünü siler (asenkron)
    }
}
