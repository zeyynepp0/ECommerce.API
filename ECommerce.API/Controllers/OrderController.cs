using ECommerce.API.Entities.Concrete;
using ECommerce.API.Services.Abstract;
using ECommerce.API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var orders = await _service.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                var order = await _service.CreateOrderAsync(orderDto);
                return Ok(new { orderId = order.Id, message = "Sipariş başarıyla oluşturuldu" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDto paymentDto)
        {
            try
            {
                // Kart bilgilerini doğrula (gerçek uygulamada ödeme sağlayıcısı ile entegrasyon)
                if (!IsValidCard(paymentDto))
                {
                    return BadRequest(new { message = "Geçersiz kart bilgileri" });
                }

                // Sipariş durumunu güncelle
                await _service.UpdateOrderStatusAsync(paymentDto.OrderId, "Paid");
                
                return Ok(new { message = "Ödeme başarıyla alındı" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Order order)
        {
            await _service.UpdateAsync(order);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
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

            return true;
        }
    }
} 