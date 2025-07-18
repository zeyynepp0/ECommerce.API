// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Sepet, ürün gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using ECommerce.API.DTO; // DTO (Data Transfer Object) sınıfları
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar
using ECommerce.API.Services.Concrete; // Servis implementasyonları

// Controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    // Sepet ürünleriyle ilgili API işlemlerini yöneten controller sınıfı
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class CartItemController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        // Sepet servis katmanı bağımlılığı
        private readonly ICartItemService _service; // Sepet işlemleri için servis

        // CartItemController constructor: Bağımlılığı enjekte eder
        public CartItemController(ICartItemService service)
        {
            _service = service; // Servisi ata
        }

        // Tüm sepet ürünlerini getirir
        [HttpGet] // GET isteğiyle çalışır
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync()); // Tüm sepet ürünlerini getir ve döndür

        // Id'ye göre sepet ürününü getirir
        [HttpGet("{id}")] // GET isteği, id parametresi ile
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id)); // İlgili sepet ürününü getir

        // Kullanıcıya ait sepet ürünlerini getirir
        [HttpGet("user/{userId}")] // GET isteği, userId parametresi ile
        public async Task<IActionResult> GetCartItemsByUserId(int userId)
        {
            var items = await _service.GetCartItemsByUserIdAsync(userId); // Kullanıcının sepet ürünlerini getir
            return Ok(items); // Sonucu döndür
        }

        // Yeni sepet ürünü ekler
        [HttpPost] // POST isteğiyle çalışır
        public async Task<IActionResult> Add(CartItemDto cartItemDto)
        {
            try
            {
                if (cartItemDto == null)
                {
                    return BadRequest("CartItem cannot be null"); // DTO null ise hata döndür
                }

                if (cartItemDto.UserId <= 0)
                {
                    return BadRequest("UserId is required and must be greater than 0"); // UserId kontrolü
                }

                if (cartItemDto.ProductId <= 0)
                {
                    return BadRequest("ProductId is required and must be greater than 0"); // ProductId kontrolü
                }

                if (cartItemDto.Quantity <= 0)
                {
                    return BadRequest("Quantity must be greater than 0"); // Quantity kontrolü
                }

                var cartItem = new CartItem
                {
                    UserId = cartItemDto.UserId, // Kullanıcı ID'si ata
                    ProductId = cartItemDto.ProductId, // Ürün ID'si ata
                    Quantity = cartItemDto.Quantity // Adet ata
                };

                await _service.AddAsync(cartItem); // Sepet ürününü ekle
                return Ok(new { message = "Ürün sepete başarıyla eklendi" }); // Başarı mesajı döndür
            }
            catch (Exception ex)
            {
                return BadRequest($"Sepete ekleme hatası: {ex.Message}"); // Hata mesajı döndür
            }
        }

        // Sepet ürününü günceller
        [HttpPut] // PUT isteğiyle çalışır
        public async Task<IActionResult> Update(CartItem cartItem)
        {
            await _service.UpdateAsync(cartItem); // Sepet ürününü güncelle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        // Id'ye göre sepet ürününü siler
        [HttpDelete("{id}")] // DELETE isteği, id parametresi ile
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id); // Sepet ürününü sil
            return Ok(); // Başarılı ise 200 OK döndür
        }

        // Kullanıcının tüm sepetini temizler
        [HttpDelete("user/{userId:int}")] // DELETE isteği, userId parametresi ile
        public async Task<IActionResult> ClearUserCart(int userId)
        {
            await _service.ClearUserCartAsync(userId); // Kullanıcının sepetini temizle
            return Ok(); // Başarılı ise 200 OK döndür
        }
    }
} 