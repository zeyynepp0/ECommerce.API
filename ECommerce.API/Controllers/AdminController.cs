// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Kullanıcı, admin gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar
using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için
using Microsoft.AspNetCore.Http; // HTTP işlemleri için
using ECommerce.API.Utilities; // JWT servis gibi yardımcı sınıflar
using ECommerce.API.DTO; // DTO (Data Transfer Object) sınıfları

// Controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    // Admin işlemlerini yöneten API controller'ı
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class AdminController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        // Admin servis katmanı bağımlılığı
        private readonly IAdminService _adminService; // Admin işlemleri için servis
        // Kullanıcı servis katmanı bağımlılığı
        private readonly IUserService _userService; // Kullanıcı işlemleri için servis
        // JWT servis bağımlılığı (token üretimi için)
        private readonly JwtService _jwtService; // JWT token işlemleri

        // AdminController constructor: Bağımlılıkları enjekte eder
        public AdminController(IAdminService adminService, IUserService userService, JwtService jwtService)
        {
            _adminService = adminService; // Admin servisini ata
            _userService = userService; // Kullanıcı servisini ata
            _jwtService = jwtService; // JWT servisini ata
        }

        // Admin girişi (login) işlemi
        [HttpPost("login")] // POST isteği, login endpoint'i
        [AllowAnonymous] // Herkes erişebilir
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto loginDto)
        {
            try
            {
                var user = await _userService.GetByEmailAsync(loginDto.Email); // E-posta ile kullanıcıyı getir
                if (user == null || user.Role != UserRole.Admin) // Kullanıcı yoksa veya admin değilse
                {
                    return Unauthorized("Admin yetkisi gereklidir"); // Yetkisiz hatası döndür
                }
                // Şifre kontrolü: Eğer hash ise hash ile, değilse düz metin karşılaştırması yap
                bool passwordValid = false;
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    if (user.PasswordHash.StartsWith("$2")) // Hash'li ise
                        passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
                    else // Düz metin ise
                        passwordValid = (loginDto.Password == user.PasswordHash);
                }
                if (!passwordValid)
                {
                    return Unauthorized("Geçersiz email veya şifre"); // Hatalı şifre
                }
                var token = _jwtService.GenerateToken(user.Id, user.Role); // JWT token üret
                return Ok(new
                {
                    token = token, // Token
                    userId = user.Id, // Kullanıcı ID
                    role = user.Role.ToString(), // Rol
                    name = user.FullName // Ad Soyad
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Giriş sırasında hata oluştu: " + ex.Message); // Hata mesajı döndür
            }
        }

        // Admin bilgilerini günceller
        [HttpPut("update/{adminId}")] // PUT isteği, update endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> UpdateAdmin(int adminId, [FromBody] UpdateAdminDto dto)
        {
            var result = await _adminService.UpdateAdminAsync(adminId, dto); // Admin bilgisini güncelle
            if (!result)
                return BadRequest("Güncelleme başarısız!"); // Hata mesajı döndür
            return Ok("Güncellendi."); // Başarı mesajı döndür
        }

        // Tüm kullanıcıları getirir. Sadece Admin yetkisi gerektirir
        [HttpGet("users")] // GET isteği, users endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync(); // Tüm kullanıcıları getir
            return Ok(users); // Sonucu döndür
        }

        // Belirli bir döneme göre gelir raporu getirir. Sadece Admin yetkisi gerektirir
        [HttpGet("revenue")] // GET isteği, revenue endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> GetRevenue([FromQuery] string period)
        {
            var revenue = await _adminService.GetRevenueReportAsync(period); // Gelir raporunu getir
            return Ok(revenue); // Sonucu döndür
        }

        // Belirli gün sayısı için yeni kullanıcı sayısını getirir. Sadece Admin yetkisi gerektirir
        [HttpGet("new-users")] // GET isteği, new-users endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> GetNewUsers([FromQuery] int days = 30)
        {
            var count = await _adminService.GetNewUsersCountAsync(days); // Yeni kullanıcı sayısını getir
            return Ok(count); // Sonucu döndür
        }

        // Kullanıcı aktivitelerini getirir. Sadece Admin yetkisi gerektirir
        [HttpGet("user-activity")] // GET isteği, user-activity endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> GetUserActivity()
        {
            var activity = await _adminService.GetUserActivityAsync(); // Kullanıcı aktivitelerini getir
            return Ok(activity); // Sonucu döndür
        }

        // Tüm yorumları getirir. Sadece Admin yetkisi gerektirir
        [HttpGet("reviews")] // GET isteği, reviews endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _adminService.GetAllReviewsAsync(); // Tüm yorumları getir
            return Ok(reviews); // Sonucu döndür
        }

        // Yorumu günceller. Sadece Admin yetkisi gerektirir
        [HttpPut("review/{id}")] // PUT isteği, review endpoint'i ve id parametresi
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto dto)
        {
            var result = await _adminService.UpdateReviewAsync(id, dto); // Yorumu güncelle
            if (!result)
                return BadRequest("Yorum güncellenemedi!"); // Hata mesajı döndür
            return Ok("Yorum güncellendi."); // Başarı mesajı döndür
        }

        // Yorumu siler. Sadece Admin yetkisi gerektirir
        [HttpDelete("review/{id}")] // DELETE isteği, review endpoint'i ve id parametresi
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> DeleteReview(int id)
        {
            var result = await _adminService.DeleteReviewAsync(id); // Yorumu sil
            if (!result)
                return BadRequest("Yorum silinemedi!"); // Hata mesajı döndür
            return Ok("Yorum silindi."); // Başarı mesajı döndür
        }

        // Admin dashboard verilerini getirir. Sadece Admin yetkisi gerektirir
        [HttpGet("dashboard")] // GET isteği, dashboard endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var dashboardData = await _adminService.GetDashboardDataAsync(); // Dashboard verisini getir
                return Ok(dashboardData); // Sonucu döndür
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dashboard error: " + ex.ToString());
                return StatusCode(500, "Dashboard verisi alınırken hata: " + ex.Message); // Hata mesajı döndür
            }
        }

        /// <summary>
        /// Admin siparişin durumunu günceller (onayla, hazırla, kargoya ver, teslim et, iptal, iade, iade onayla).
        /// </summary>
        [HttpPost("order/{orderId}/status")] // api/admin/order/{orderId}/status
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromQuery] OrderStatus status)
        {
            try
            {
                var result = await _adminService.UpdateOrderStatusAsync(orderId, status);
                if (!result)
                    return BadRequest("Sipariş bulunamadı veya güncellenemedi.");
                // Kullanıcıya bildirim gönder
                var dbContext = HttpContext.RequestServices.GetService(typeof(ECommerce.API.Data.MyDbContext)) as ECommerce.API.Data.MyDbContext;
                var notificationService = HttpContext.RequestServices.GetService(typeof(ECommerce.API.Services.Abstract.INotificationService)) as ECommerce.API.Services.Abstract.INotificationService;
                if (dbContext != null && notificationService != null)
                {
                    var order = dbContext.Orders.FirstOrDefault(o => o.Id == orderId);
                    if (order != null)
                    {
                        await notificationService.AddAsync(new ECommerce.API.DTO.NotificationDto
                        {
                            UserId = order.UserId,
                            Message = $"Siparişinizin durumu güncellendi: {status}",
                            IsRead = false,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
                return Ok(new { message = $"Sipariş durumu güncellendi: {status}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Admin sipariş iptalini onaylar.
        /// </summary>
        [HttpPost("order/{orderId}/cancel/approve")] // api/admin/order/{orderId}/cancel/approve
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveOrderCancel(int orderId)
        {
            try
            {
                var dbContext = HttpContext.RequestServices.GetService(typeof(ECommerce.API.Data.MyDbContext)) as ECommerce.API.Data.MyDbContext;
                var order = dbContext?.Orders.FirstOrDefault(o => o.Id == orderId);
                if (order == null)
                    return BadRequest("Sipariş bulunamadı veya güncellenemedi.");
                order.Status = ECommerce.API.Entities.Concrete.OrderStatus.Cancelled;
                order.UserRequest = ECommerce.API.Entities.Concrete.UserOrderRequest.None;
                order.AdminStatus = ECommerce.API.Entities.Concrete.AdminOrderStatus.Approved;
                dbContext.Orders.Update(order);
                dbContext.SaveChanges();
                return Ok(new { message = "Sipariş iptali onaylandı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Admin iade sürecini başlatır (iade onaylandı, geri ödeme başlatıldı).
        /// </summary>
        [HttpPost("order/{orderId}/refund/approve")] // api/admin/order/{orderId}/refund/approve
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveOrderRefund(int orderId)
        {
            try
            {
                var dbContext = HttpContext.RequestServices.GetService(typeof(ECommerce.API.Data.MyDbContext)) as ECommerce.API.Data.MyDbContext;
                var order = dbContext?.Orders.FirstOrDefault(o => o.Id == orderId);
                if (order == null)
                    return BadRequest("Sipariş bulunamadı veya güncellenemedi.");
                order.Status = ECommerce.API.Entities.Concrete.OrderStatus.Refunded;
                order.UserRequest = ECommerce.API.Entities.Concrete.UserOrderRequest.None;
                order.AdminStatus = ECommerce.API.Entities.Concrete.AdminOrderStatus.Approved;
                dbContext.Orders.Update(order);
                dbContext.SaveChanges();
                return Ok(new { message = "İade onaylandı, geri ödeme başlatıldı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Admin siparişin admin durumunu günceller.
        /// </summary>
        [HttpPost("order/{orderId}/admin-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderAdminStatus(int orderId, [FromQuery] Entities.Concrete.AdminOrderStatus adminStatus)
        {
            try
            {
                await _adminService.UpdateOrderAdminStatusAsync(orderId, adminStatus);
                return Ok(new { message = $"Sipariş durumu güncellendi: {adminStatus}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Tüm siparişleri getirir. Sadece Admin yetkisi gerektirir
        [HttpGet("orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _adminService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPost("order/{orderId}/user-request/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectUserRequest(int orderId)
        {
            var dbContext = HttpContext.RequestServices.GetService(typeof(ECommerce.API.Data.MyDbContext)) as ECommerce.API.Data.MyDbContext;
            var order = dbContext?.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                return BadRequest(new { message = "Sipariş bulunamadı." });
            order.UserRequest = ECommerce.API.Entities.Concrete.UserOrderRequest.None;
            order.AdminStatus = ECommerce.API.Entities.Concrete.AdminOrderStatus.Rejected;
            dbContext.Orders.Update(order);
            dbContext.SaveChanges();
            return Ok(new { message = "Kullanıcı isteği reddedildi." });
        }
    }
} 