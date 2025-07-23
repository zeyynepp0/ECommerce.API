// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Ürün, kategori gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using ECommerce.API.Services.Concrete; // Servis implementasyonları
using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar
using ECommerce.API.DTO; // DTO (Data Transfer Object) sınıfları

// Controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    // Ürünlerle ilgili API işlemlerini yöneten controller sınıfı
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class ProductController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        // Ürün servis katmanı bağımlılığı
        private readonly IProductService _service; // Ürün işlemleri için servis

        // ProductController constructor: Bağımlılığı enjekte eder
        public ProductController(IProductService service)
        {
            _service = service; // Servisi ata
        }

        // Tüm ürünleri getirir
        [HttpGet] // GET isteğiyle çalışır
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync()); // Tüm ürünleri getir ve döndür

        // Id'ye göre ürünü getirir
        [HttpGet("{id}")] // GET isteği, id parametresi ile
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id)); // İlgili ürünü getir

        

        // DTO ile yeni ürün ekler. Sadece Admin yetkisi gerektirir
        [HttpPost("add")] // POST isteği, add endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> AddProduct([FromBody] ProductDto dto)
        {
            try
            {
                await _service.AddProductAsync(dto); // Ürünü ekle
                return Ok("Ürün eklendi."); // Başarı mesajı döndür
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Hata mesajı ile birlikte 400 döndür
            }
        }

     

        // DTO ile ürünü günceller. Sadece Admin yetkisi gerektirir
        [HttpPut("update/{id}")] // PUT isteği, update endpoint'i ve id parametresi
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            try
            {
                await _service.UpdateProductAsync(id, dto); // Ürünü güncelle
                return Ok("Ürün güncellendi."); // Başarı mesajı döndür
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Hata mesajı ile birlikte 400 döndür
            }
        }

        // Id'ye göre ürünü siler. Sadece Admin yetkisi gerektirir
        [HttpDelete("delete/{id}")] // DELETE isteği, delete endpoint'i ve id parametresi
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _service.DeleteProductAsync(id); // Ürünü sil
            return Ok("Ürün silindi."); // Başarı mesajı döndür
        }

        // Belirli bir kategoriye ait ve belirli bir ürünü hariç tutarak ürünleri getirir
        [HttpGet("by-category")] // GET isteği, by-category endpoint'i
        public async Task<IActionResult> GetByCategory([FromQuery] int categoryId ,[FromQuery] int excludeProductId)
        {
            var products = await _service.GetByCategoryIdAsync(categoryId,  excludeProductId); // Kategoriye göre ürünleri getir, bir ürünü hariç tut
            return Ok(products); // Sonucu döndür
        }

        // İlgili ürünün kategorisindeki diğer ürünleri getirir
        [HttpGet("related/{id}")] // GET isteği, related endpoint'i ve id parametresi
        public async Task<IActionResult> GetRelatedProducts(int id)
        {
            var product = await _service.GetByIdAsync(id); // Ürünü getir
            if (product == null) return NotFound(); // Ürün yoksa 404 döndür

            var related = await _service.GetByCategoryIdAsync(product.CategoryId, product.Id); // Aynı kategorideki diğer ürünleri getir
            return Ok(related); // Sonucu döndür
        }

        // Ürün resmi yükleme endpointi
        // Açıklama: Admin, ürün eklerken/güncellerken bu endpoint ile görsel yükleyebilir.
        // Yüklenen dosya sunucuda /Image/ProductsImage/ klasörüne ürün adıyla benzersiz isimle kaydedilir ve dosya yolu döndürülür.
        [HttpPost("upload-image")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadProductImage([FromForm] IFormFile image, [FromForm] string productName)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Resim dosyası seçilmedi.");

            // Ürün adını dosya adının başına ekle, güvenli hale getir
            var safeProductName = string.IsNullOrWhiteSpace(productName) ? "Urun" : productName.Replace(" ", "_").Replace("/", "_");
            var extension = Path.GetExtension(image.FileName);
            var fileName = $"{safeProductName}_{Guid.NewGuid()}{extension}";
            // Kayıt yolu (düzeltildi: fazladan ECommerce.API eklenmiyor)
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "Image", "ProductsImage", fileName);
            // Klasör yoksa oluştur
            var dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            // Dosyayı kaydet
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            // API'de kullanılacak yol (frontend için)
            var relativePath = $"/Image/ProductsImage/{fileName}";
            return Ok(new { imageUrl = relativePath });
        }
    }
} 