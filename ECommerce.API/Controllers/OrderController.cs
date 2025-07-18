// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Sipariş gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using ECommerce.API.DTO; // DTO (Data Transfer Object) sınıfları
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar

// Controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class OrderController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        private readonly IOrderService _service; // Sipariş işlemleri için servis
        public OrderController(IOrderService service)
        {
            _service = service; // Servisi ata
        }

        [HttpGet] // Tüm siparişleri getirir
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync()); // Tüm siparişleri getir ve döndür

        [HttpGet("{id}")] // Id'ye göre siparişi getirir
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id)); // İlgili siparişi getir

        [HttpGet("user/{userId:int}")] // Kullanıcıya ait siparişleri getirir
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var orders = await _service.GetOrdersByUserIdAsync(userId); // Kullanıcının siparişlerini DTO olarak getir
            return Ok(orders); // Sonucu döndür
        }

        [HttpPost] // Yeni sipariş oluşturur
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                var order = await _service.CreateOrderAsync(orderDto); // Siparişi oluştur
                return Ok(new { orderId = order.Id, message = "Sipariş başarıyla oluşturuldu" }); // Başarı mesajı döndür
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // Hata mesajı döndür
            }
        }

        [HttpPost("payment")] // Sipariş ödemesini işler
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDto paymentDto)
        {
            try
            {
                // Kart bilgilerini doğrula (gerçek uygulamada ödeme sağlayıcısı ile entegrasyon)
                if (!IsValidCard(paymentDto))
                {
                    return BadRequest(new { message = "Geçersiz kart bilgileri" }); // Hatalı kart bilgisi
                }

                // Sipariş durumunu güncelle
                await _service.UpdateOrderStatusAsync(paymentDto.OrderId, "Paid"); // Siparişi ödenmiş olarak işaretle
                
                return Ok(new { message = "Ödeme başarıyla alındı" }); // Başarı mesajı döndür
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // Hata mesajı döndür
            }
        }

        [HttpPut] // Siparişi günceller
        public async Task<IActionResult> Update(Order order)
        {
            await _service.UpdateAsync(order); // Siparişi güncelle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        [HttpDelete("{id}")] // Id'ye göre siparişi siler
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id); // Siparişi sil
            return Ok(); // Başarılı ise 200 OK döndür
        }

        private bool IsValidCard(PaymentDto payment)
        {
            // Basit kart doğrulama (gerçek uygulamada daha kapsamlı olmalı)
            if (string.IsNullOrEmpty(payment.CardNumber) || payment.CardNumber.Length < 13 || payment.CardNumber.Length > 19)
                return false;
            
            if (string.IsNullOrEmpty(payment.CardHolderName))
                return false;
            
            if (string.IsNullOrEmpty(payment.ExpiryMonth) || string.IsNullOrEmpty(payment.ExpiryYear))
                return false;
            
            if (string.IsNullOrEmpty(payment.Cvv) || payment.Cvv.Length < 3 || payment.Cvv.Length > 4)
                return false;

            // Ay kontrolü
            if (!int.TryParse(payment.ExpiryMonth, out int month) || month < 1 || month > 12)
                return false;

            // Yıl kontrolü
            if (!int.TryParse(payment.ExpiryYear, out int year) || year < DateTime.Now.Year)
                return false;

            return true; // Kart geçerli ise true döndür
        }
    }
} 