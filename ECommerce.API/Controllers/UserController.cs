// using direktifleri: Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Kullanıcı, sipariş gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar
using ECommerce.API.Utilities; // JWT servis gibi yardımcı sınıflar
using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için
using ECommerce.API.DTO;

// Proje içindeki controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    // Kullanıcılarla ilgili API işlemlerini yöneten controller sınıfı
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class UserController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        // Kullanıcı servis katmanı bağımlılığı
        private readonly IUserService _service; // Kullanıcı işlemleri için servis
        // JWT servis bağımlılığı (token üretimi için)
        private readonly JwtService _jwtService; // JWT token işlemleri
        // Sipariş servis katmanı bağımlılığı
        private readonly IOrderService _orderService; // Sipariş işlemleri için servis
        // Yorum servis katmanı bağımlılığı
        private readonly IReviewService _reviewService; // Yorum işlemleri için servis

        // UserController constructor: Bağımlılıkları enjekte eder
        public UserController(IUserService service, JwtService jwtService, IOrderService orderService, IReviewService reviewService)
        {
            _service = service; // Kullanıcı servisini ata
            _jwtService = jwtService; // JWT servisini ata
            _orderService = orderService; // Sipariş servisini ata
            _reviewService = reviewService; // Yorum servisini ata
        }

        // Tüm kullanıcıları getirir. Sadece Admin veya User yetkisi gerektirir.
        [HttpGet] // GET isteğiyle çalışır
        [Authorize(Roles = "Admin,User")] // Sadece Admin ve User rolleri erişebilir
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            var dtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role.ToString(),
                EmailConfirmed = u.EmailConfirmed,
                BirthDate = u.BirthDate,
                OrderCount = u.Orders?.Count ?? 0,
                TotalSpent = u.Orders?.Sum(o => o.TotalAmount) ?? 0,
                IsActive = u.IsActive
            }).ToList();
            return Ok(dtos);
        }

        // Id'ye göre kullanıcıyı getirir
        [HttpGet("{id}")] // GET isteği, id parametresi ile
        public async Task<IActionResult> GetById(int id)
        {
            var u = await _service.GetByIdAsync(id);
            if (u == null) return NotFound();
            var dto = new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role.ToString(),
                EmailConfirmed = u.EmailConfirmed,
                BirthDate = u.BirthDate,
                OrderCount = u.Orders?.Count ?? 0,
                TotalSpent = u.Orders?.Sum(o => o.TotalAmount) ?? 0,
                IsActive = u.IsActive
            };
            return Ok(dto);
        }

        // Yeni kullanıcı ekler
        [HttpPost] // POST isteğiyle çalışır
        public async Task<IActionResult> Add([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _service.AddAsync(user); // Kullanıcıyı ekle
                return Ok(user.Id); // Kullanıcı eklendikten sonra ID'yi döndür
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Kullanıcı ekleme örnek JSON'u aşağıda açıklama olarak verilmiş
        //{
        //  "firstName": "Zeynep",
        //  "lastName": "Can",
        //  "email": "zeynep@example.com",
        //  "passwordHash": "123456",
        //  "addresses": [],
        //  "orders": [],
        //  "reviews": []
        //}

        // Kullanıcı girişi (login) işlemi
        [HttpPost("login")] // POST isteği, login endpoint'i
        [AllowAnonymous] // Herkes erişebilir
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _service.AuthenticateAsync(request.Email, request.Password); // Kullanıcıyı doğrula
                if (user == null) // Kullanıcı bulunamazsa
                    return Unauthorized(new { message = "Geçersiz e-posta veya şifre." }); // Yetkisiz hatası döndür
                var token = _jwtService.GenerateToken(user.Id, user.Role); // JWT token üret
                return Ok(new { token }); // Token'ı döndür
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message ?? "Bilinmeyen bir hata oluştu." });
            }
        }

        // Kullanıcı için yeni sipariş oluşturur
        [HttpPost("orders")] // POST isteği, orders endpoint'i
        [Authorize(Roles = "User")] // Sadece User rolü erişebilir
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            await _orderService.AddAsync(order); // Siparişi ekle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        // Kullanıcı için yeni yorum ekler
        [HttpPost("reviews")] // POST isteği, reviews endpoint'i
        [Authorize(Roles = "User")] // Sadece User rolü erişebilir
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            await _reviewService.AddAsync(review); // Yorumu ekle
            return Ok(); // Başarılı ise 200 OK döndür
        }

        // Kullanıcının geçmiş siparişlerini getirir
        [HttpGet("orders/history/{userId}")] // GET isteği, userId parametresi ile
        [Authorize(Roles = "User")] // Sadece User rolü erişebilir
        public async Task<IActionResult> GetOrderHistory(int userId)
        {
            var orders = await _orderService.GetByUserIdAsync(userId); // Kullanıcının siparişlerini getir
            return Ok(orders); // Siparişleri döndür
        }

        // Kullanıcıyı aktif/pasif yapar
        [HttpPost("set-active")]
        public async Task<IActionResult> SetActive([FromBody] SetActiveRequest req)
        {
            await _service.SetActiveAsync(req.Id, req.IsActive);
            return Ok();
        }
        public class SetActiveRequest
        {
            public int Id { get; set; }
            public bool IsActive { get; set; }
        }

        //// Kullanıcıyı günceller
        //[HttpPut] // PUT isteğiyle çalışır
        //public async Task<IActionResult> Update(User user)
        //{
        //    await _service.UpdateAsync(user); // Kullanıcıyı güncelle
        //    return Ok(); // Başarılı ise 200 OK döndür
        //}

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
        {
            try
            {
                await _service.UpdateAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Şifremi unuttum endpoint'i
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            try
            {
                await _service.ForgotPasswordAsync(req.Email);
                return Ok(new { message = "Şifre sıfırlama talimatı e-posta adresinize gönderildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Şifre sıfırlama endpoint'i
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
        {
            try
            {
                await _service.ResetPasswordAsync(req.Token, req.NewPassword);
                return Ok(new { message = "Şifre başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // E-posta doğrulama endpoint'i
        [HttpPost("verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest req)
        {
            Console.WriteLine($"VerifyEmail: token={req.Token}");
            var users = await _service.GetAllAsync();
            var user = users.FirstOrDefault(u => u.EmailVerificationToken == req.Token && u.EmailVerificationTokenExpires > DateTime.UtcNow);
            if (user == null)
            {
                Console.WriteLine("Kullanıcı bulunamadı veya token süresi dolmuş.");
                return BadRequest(new { message = "Geçersiz veya süresi dolmuş doğrulama linki." });
            }
            Console.WriteLine($"Kullanıcı bulundu: {user.Email}, EmailConfirmed={user.EmailConfirmed}");
            user.EmailConfirmed = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpires = null;
            await _service.UpdateAsync(user);
            Console.WriteLine($"Kullanıcı doğrulandı: {user.Email}");
            return Ok(new { message = "E-posta adresiniz başarıyla doğrulandı." });
        }
        public class VerifyEmailRequest
        {
            public string Token { get; set; } = string.Empty;
        }

        public class ForgotPasswordRequest
        {
            public string Email { get; set; } = string.Empty;
        }
        public class ResetPasswordRequest
        {
            public string Token { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
        }

        // Id'ye göre kullanıcıyı siler
        [HttpDelete("{id}")] // DELETE isteği, id parametresi ile
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id); // Kullanıcıyı sil
            return Ok(); // Başarılı ise 200 OK döndür
        }
    }

    // Kullanıcı login isteği için kullanılan model
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
} 