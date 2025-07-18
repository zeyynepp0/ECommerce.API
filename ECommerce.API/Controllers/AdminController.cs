// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Kullanıcı, admin gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar
using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için
using ECommerce.API.Services.Abstract; // Servis arayüzleri (tekrar)
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
        [HttpPut("update")] // PUT isteği, update endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> UpdateAdmin([FromBody] UpdateAdminDto dto)
        {
            var result = await _adminService.UpdateAdminAsync(dto); // Admin bilgisini güncelle
            if (!result)
                return BadRequest("Güncelleme başarısız!"); // Hata mesajı döndür
            return Ok("Güncellendi."); // Başarı mesajı döndür
        }

        // Tüm kullanıcıları getirir. Sadece Admin yetkisi gerektirir
        [HttpGet("users")] // GET isteği, users endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync(); // Tüm kullanıcıları getir
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

        // Belirli bir ay ve yıl için yeni kullanıcı sayısını getirir. Sadece Admin yetkisi gerektirir
        [HttpGet("new-users")] // GET isteği, new-users endpoint'i
        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        public async Task<IActionResult> GetNewUsers([FromQuery] int month, [FromQuery] int year)
        {
            var count = await _adminService.GetNewUsersCountAsync(month, year); // Yeni kullanıcı sayısını getir
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
    }
} 