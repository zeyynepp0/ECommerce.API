// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Kategori gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar
using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için
using ECommerce.API.DTO; // DTO (Data Transfer Object) sınıfları

// Controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    // Kategorilerle ilgili API işlemlerini yöneten controller sınıfı
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class CategoryController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        // Kategori servis katmanı bağımlılığı
        private readonly ICategoryService _service; // Kategori işlemleri için servis

        // CategoryController constructor: Bağımlılığı enjekte eder
        public CategoryController(ICategoryService service)
        {
            _service = service; // Servisi ata
        }

        // Tüm kategorileri getirir (DTO ile)
        [HttpGet] // GET isteğiyle çalışır
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync()); // Tüm kategorileri DTO olarak getir ve döndür

        // Id'ye göre kategoriyi getirir (DTO ile). Sadece Admin yetkisi gerektirir
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // Yeni kategori ekler. Sadece Admin yetkisi gerektirir
        [HttpPost("add")] // POST isteği, add endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto dto)
        {
            await _service.AddCategoryAsync(dto); // Kategoriyi ekle
            return Ok("Kategori eklendi."); // Başarı mesajı döndür
        }

        // Kategoriyi günceller. Sadece Admin yetkisi gerektirir
        [HttpPut("update/{id}")] // PUT isteği, update endpoint'i ve id parametresi
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
        {
            await _service.UpdateCategoryAsync(id, dto); // Kategoriyi güncelle
            return Ok("Kategori güncellendi."); // Başarı mesajı döndür
        }

        // Id'ye göre kategoriyi siler. Sadece Admin yetkisi gerektirir
        [HttpDelete("delete/{id}")] // DELETE isteği, delete endpoint'i ve id parametresi
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _service.DeleteCategoryAsync(id); // Kategoriyi sil
            return Ok("Kategori silindi."); // Başarı mesajı döndür
        }

        // Kategori resmi yükleme endpointi
        // Açıklama: Admin, kategori eklerken/güncellerken bu endpoint ile görsel yükleyebilir.
        // Yüklenen dosya sunucuda /Image/CategoryImage/ klasörüne benzersiz isimle kaydedilir ve dosya yolu döndürülür.
        [HttpPost("upload-image")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadCategoryImage([FromForm] IFormFile image, [FromForm] string categoryName)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Resim dosyası seçilmedi.");

            // Kategori adını dosya adının başına ekle, güvenli hale getir
            var safeCategoryName = string.IsNullOrWhiteSpace(categoryName) ? "Kategori" : categoryName.Replace(" ", "_").Replace("/", "_");
            var extension = Path.GetExtension(image.FileName);
            var fileName = $"{safeCategoryName}_{Guid.NewGuid()}{extension}";

            // Uygulamanın çalışma dizinini ve kaydedilecek yolu logla
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "Image", "CategoryImage");
            Console.WriteLine($"[LOG] Çalışma dizini: {Directory.GetCurrentDirectory()}");
            Console.WriteLine($"[LOG] Kategori görseli kaydedilecek klasör: {rootPath}");

            // Klasör yoksa oluştur
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var savePath = Path.Combine(rootPath, fileName);
            Console.WriteLine($"[LOG] Kategori görseli tam dosya yolu: {savePath}");

            try
            {
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla ve kullanıcıya bilgi ver
                Console.WriteLine($"[ERROR] Görsel kaydedilemedi: {ex.Message}");
                return StatusCode(500, "Görsel kaydedilemedi: " + ex.Message);
            }

            // Frontend ve veritabanı için göreli yol
            var relativePath = $"/Image/CategoryImage/{fileName}";
            return Ok(new { imageUrl = relativePath });
        }
    }
} 