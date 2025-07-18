// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Sipariş kalemi gibi varlık sınıfları
using ECommerce.API.Repository.Abstract; // Repository arayüzleri
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar

// Controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class OrderItemController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        private readonly IOrderItemService _service; // Sipariş kalemi işlemleri için servis
        public OrderItemController(IOrderItemService service)
        {
            _service = service; // Servisi ata
        }

        [HttpGet] // Tüm sipariş kalemlerini getirir
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync()); // Tüm sipariş kalemlerini getir ve döndür

        [HttpGet("{id}")] // Id'ye göre sipariş kalemini getirir
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id)); // İlgili sipariş kalemini getir

        [HttpPost] // Yeni sipariş kalemi ekler
        public async Task<IActionResult> Add([FromBody] OrderItem orderItem)
        {
            await _service.AddAsync(orderItem); // Sipariş kalemini ekle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        [HttpPut] // Sipariş kalemini günceller
        public async Task<IActionResult> Update([FromBody] OrderItem orderItem)
        {
            await _service.UpdateAsync(orderItem); // Sipariş kalemini güncelle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        [HttpDelete("{id}")] // Id'ye göre sipariş kalemini siler
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id); // Sipariş kalemini sil
            return Ok(); // Başarılı ise 200 OK döndür
        }
    }
} 