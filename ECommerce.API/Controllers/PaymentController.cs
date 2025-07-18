// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Ödeme gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar

// Controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class PaymentController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        private readonly IPaymentService _service; // Ödeme işlemleri için servis
        public PaymentController(IPaymentService service)
        {
            _service = service; // Servisi ata
        }

        [HttpGet] // Tüm ödemeleri getirir
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync()); // Tüm ödemeleri getir ve döndür

        [HttpGet("{id}")] // Id'ye göre ödemeyi getirir
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id)); // İlgili ödemeyi getir

        [HttpPost] // Yeni ödeme ekler
        public async Task<IActionResult> Add([FromBody] Payment payment)
        {
            await _service.AddAsync(payment); // Ödemeyi ekle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        [HttpPut] // Ödemeyi günceller
        public async Task<IActionResult> Update([FromBody] Payment payment)
        {
            await _service.UpdateAsync(payment); // Ödemeyi güncelle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        [HttpDelete("{id}")] // Id'ye göre ödemeyi siler
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id); // Ödemeyi sil
            return Ok(); // Başarılı ise 200 OK döndür
        }
    }
} 