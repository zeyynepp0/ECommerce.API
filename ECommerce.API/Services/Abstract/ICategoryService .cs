// Kategori servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Kategori varlık sınıfı
using ECommerce.API.DTO; // DTO sınıfları

namespace ECommerce.API.Services.Abstract
{
    // Kategori işlemleri için servis arayüzü
    public interface ICategoryService
    {
        // Tüm kategorileri DTO olarak getirir
        Task<List<CategoryDto>> GetAllAsync();
        // Id'ye göre kategoriyi DTO olarak getirir
        Task<CategoryDto> GetByIdAsync(int id);
        Task AddAsync(Category category); // Yeni kategori ekler (asenkron)
        Task UpdateAsync(Category category); // Kategoriyi günceller (asenkron)
        Task DeleteAsync(int id); // Kategoriyi siler (asenkron)
        Task AddCategoryAsync(CategoryDto dto); // Yeni kategori ekler (DTO ile, asenkron)
        Task UpdateCategoryAsync(int id, CategoryDto dto); // Kategoriyi günceller (DTO ile, asenkron)
        Task DeleteCategoryAsync(int id); // Kategoriyi siler (DTO ile, asenkron)

        // --- Eski entity döndüren fonksiyonlar (gerekirse kullanılır) ---
        Task<List<Category>> GetAllEntityAsync();
        Task<Category> GetByIdEntityAsync(int id);
    }
}
