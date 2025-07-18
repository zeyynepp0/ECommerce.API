// using direktifleri: Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Entities.Concrete; // Kullanıcı, sipariş gibi varlık sınıfları
using ECommerce.API.Services.Abstract; // Servis arayüzleri
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar
using ECommerce.API.Utilities; // JWT servis gibi yardımcı sınıflar
using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için

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
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync()); // Tüm kullanıcıları getir ve döndür

        // Id'ye göre kullanıcıyı getirir
        [HttpGet("{id}")] // GET isteği, id parametresi ile
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id)); // İlgili kullanıcıyı getir

        // Yeni kullanıcı ekler
        [HttpPost] // POST isteğiyle çalışır
        public async Task<IActionResult> Add([FromBody] User user)
        {
            await _service.AddAsync(user); // Kullanıcıyı ekle
            return Ok(user.Id); // Kullanıcı eklendikten sonra ID'yi döndür
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
            var user = await _service.AuthenticateAsync(request.Email, request.Password); // Kullanıcıyı doğrula
            if (user == null) // Kullanıcı bulunamazsa
                return Unauthorized("Geçersiz e-posta veya şifre."); // Yetkisiz hatası döndür
            var token = _jwtService.GenerateToken(user.Id, user.Role); // JWT token üret
            return Ok(new { token }); // Token'ı döndür
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

        // Kullanıcıyı günceller
        [HttpPut] // PUT isteğiyle çalışır
        public async Task<IActionResult> Update(User user)
        {
            await _service.UpdateAsync(user); // Kullanıcıyı güncelle
            return Ok(); // Başarılı ise 200 OK döndür
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
        public string Email { get; set; } // E-posta adresi
        public string Password { get; set; } // Şifre
    }
} 