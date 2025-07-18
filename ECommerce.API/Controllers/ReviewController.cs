// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Yorum gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için

// Controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class ReviewController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        private readonly IReviewService _service; // Yorum işlemleri için servis
        public ReviewController(IReviewService service)
        {
            _service = service; // Servisi ata
        }

        [HttpGet] // Tüm yorumları getirir
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false) => Ok(await _service.GetAllAsync(includeDeleted)); // Tüm yorumları getir ve döndür

        [HttpGet("{id}")] // Id'ye göre yorumu getirir
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id)); // İlgili yorumu getir

        [HttpPost] // Yeni yorum ekler
        public async Task<IActionResult> Add([FromBody] DTO.ReviewDto dto)
        {
            if (dto.UserId == 0 || dto.ProductId == 0 || string.IsNullOrWhiteSpace(dto.Comment) || dto.Rating < 1)
                return BadRequest("Eksik veya hatalı veri!");
            try
            {
                var review = new Entities.Concrete.Review
                {
                    ProductId = dto.ProductId,
                    UserId = dto.UserId,
                    Comment = dto.Comment,
                    Rating = dto.Rating,
                    CreatedAt = DateTime.UtcNow
                };
                await _service.AddAsync(review); // Yorumu ekle
                return Ok(review); // Eklenen yorumu döndür
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Yorum eklenirken hata oluştu: " + ex.Message);
            }
        }

        [HttpPut] // Yorumu günceller
        public async Task<IActionResult> Update([FromBody] DTO.UpdateReviewDto dto)
        {
            await _service.UpdateAsync(dto); // Yorumu güncelle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        [HttpDelete("{id}")] // Id'ye göre yorumu siler
        public async Task<IActionResult> Delete(int id, [FromQuery] string deletedBy)
        {
            await _service.SoftDeleteAsync(id, deletedBy); // Yorumu silindi olarak işaretle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        [HttpGet("by-product")] // Ürüne ait yorumları getirir
        public async Task<IActionResult> GetReviewsByProduct([FromQuery] int productId)
        {
            var reviews = await _service.GetByProductIdAsync(productId); // Ürüne ait yorumları getir
            return Ok(reviews); // Sonucu döndür
        }
    }
}