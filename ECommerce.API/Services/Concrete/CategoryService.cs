// Kategori servisinin iş mantığı ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Kategori varlık sınıfı
using ECommerce.API.Repository.Abstract; // Kategori repository arayüzü
using ECommerce.API.Services.Abstract; // Kategori servis arayüzü
using ECommerce.API.DTO; // DTO sınıfları
using ECommerce.API.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Services.Concrete
{
    // Kategorilerle ilgili iş mantığını yöneten servis sınıfı
    public class CategoryService : ICategoryService
    {
        // Kategori repository'si (veri erişim katmanı)
        private readonly ICategoryRepository _repo;
        private readonly MyDbContext _context;

        // CategoryService constructor: Repository bağımlılığını enjekte eder
        public CategoryService(ICategoryRepository repo, MyDbContext context)
        {
            _repo = repo; // Repository'yi ata
            _context = context;
        }

        // Tüm kategorileri DTO olarak getirir
        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUrl
            }).ToList();
        }

        // Id'ye göre kategoriyi DTO olarak getirir
        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new CategoryDto { Id = c.Id, Name = c.Name, ImageUrl = c.ImageUrl };
        }

        // --- Eski entity döndüren fonksiyonlar (gerekirse kullanılır) ---
        public async Task<List<Category>> GetAllEntityAsync() => await _repo.GetAllAsync();
        public async Task<Category> GetByIdEntityAsync(int id) => await _repo.GetByIdAsync(id);

        // Yeni kategori ekler
        public async Task AddAsync(Category category)
        {
            await _repo.AddAsync(category); // Kategoriyi ekle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // Var olan kategoriyi günceller
        public async Task UpdateAsync(Category category)
        {
            _repo.Update(category); // Kategoriyi güncelle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // Id'ye göre kategoriyi siler (varsa)
        public async Task DeleteAsync(int id)
        {
            // O kategoriye ait ürün var mı kontrol et
            var hasProduct = await _context.Products.AnyAsync(p => p.CategoryId == id && p.IsActive);
            if (hasProduct)
                throw new Exception("Bu kategoriye ait ürünler olduğu için silinemez.");
            var category = await _repo.GetByIdAsync(id);
            if (category != null)
            {
                _repo.Delete(category);
                await _repo.SaveAsync();
            }
        }

        // DTO ile yeni kategori ekler
        public async Task AddCategoryAsync(CategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name, // Kategori adı
                ImageUrl = dto.ImageUrl // Kategori görseli
            };
            await _repo.AddAsync(category); // Kategoriyi ekle
            await _repo.SaveAsync(); // Değişiklikleri kaydet
        }

        // DTO ile var olan kategoriyi günceller
        public async Task UpdateCategoryAsync(int id, CategoryDto dto)
        {
            var category = await _repo.GetByIdAsync(id); // Kategoriyi getir
            if (category != null)
            {
                category.Name = dto.Name; // Adı güncelle
                category.ImageUrl = dto.ImageUrl; // Görseli güncelle
                _repo.Update(category); // Kategoriyi güncelle
                await _repo.SaveAsync(); // Değişiklikleri kaydet
            }
        }

        // DTO ile kategoriyi siler (varsa)
        public async Task DeleteCategoryAsync(int id)
        {
            // O kategoriye ait aktif ürün var mı kontrol et
            var hasProduct = await _context.Products.AnyAsync(p => p.CategoryId == id && p.IsActive);
            if (hasProduct)
                throw new Exception("Bu kategoriye ait ürünler olduğu için silinemez.");
            var category = await _repo.GetByIdAsync(id); // Kategoriyi getir
            if (category != null)
            {
                _repo.Delete(category); // Kategoriyi sil
                await _repo.SaveAsync(); // Değişiklikleri kaydet
            }
        }
    }
}
