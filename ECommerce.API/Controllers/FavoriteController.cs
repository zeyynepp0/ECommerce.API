// Gerekli kütüphaneleri projeye dahil eder
using ECommerce.API.Services.Abstract; // Favori servis arayüzü
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıflar
using ECommerce.API.DTO; // DTO (Data Transfer Object) sınıfları

// Controller'ların bulunduğu namespace
namespace ECommerce.API.Controllers
{
    // Kullanıcıların favori ürün işlemlerini yöneten API controller'ı
    [ApiController] // Bu sınıfın bir API controller olduğunu belirtir
    [Route("api/[controller]")] // API endpoint rotasını ayarlar
    public class FavoriteController : ControllerBase // ControllerBase'den türeyen temel controller
    {
        // Favori servis katmanı bağımlılığı
        private readonly IFavoriteService _service; // Favori işlemleri için servis

        // Controller constructor'ı: Bağımlılığı enjekte eder
        public FavoriteController(IFavoriteService service)
        {
            _service = service; // Servisi ata
        }

        // Kullanıcının favori ürünlerini getirir
        [HttpGet("user/{userId:int}")] // GET isteği, userId parametresi ile
        public async Task<IActionResult> GetUserFavorites(int userId)
        {
            var favorites = await _service.GetFavoritesByUserIdAsync(userId); // Kullanıcının favori ürünlerini getir
            return Ok(favorites); // Sonucu döndür
        }

        // Kullanıcının favorisine ürün ekler
        [HttpPost("add")] // POST isteği, add endpoint'i
        public async Task<IActionResult> AddToFavorites([FromBody] AddFavoriteRequest request)
        {
            var (success, message) = await _service.AddToFavoritesAsync(request.UserId, request.ProductId); // Favoriye ekle
            if (success) // Başarılıysa
            {
                return Ok(new { message }); // Başarı mesajı döndür
            }
            return BadRequest(new { message }); // Hata mesajı döndür
        }

        // Kullanıcının favorisinden ürün kaldırır
        [HttpDelete("remove")] // DELETE isteği, remove endpoint'i
        public async Task<IActionResult> RemoveFromFavorites([FromBody] RemoveFavoriteRequest request)
        {
            var result = await _service.RemoveFromFavoritesAsync(request.UserId, request.ProductId); // Favoriden kaldır
            if (result) // Başarılıysa
            {
                return Ok(new { message = "Ürün favorilerden kaldırıldı" }); // Başarı mesajı döndür
            }
            return BadRequest(new { message = "Ürün favorilerde bulunamadı" }); // Hata mesajı döndür
        }

        // Kullanıcının belirtilen ürünü favori olarak ekleyip eklemediğini kontrol eder
        [HttpGet("check/{userId:int}/{productId:int}")] // GET isteği, userId ve productId parametreleri ile
        public async Task<IActionResult> CheckIfFavorited(int userId, int productId)
        {
            var isFavorited = await _service.IsProductFavoritedByUserAsync(userId, productId); // Favori kontrolü yap
            return Ok(new { isFavorited }); // Sonucu döndür
        }

        // Favorilere ürün ekleme isteği için kullanılan model
        public class AddFavoriteRequest
        {
            public int UserId { get; set; } // Kullanıcı ID'si
            public int ProductId { get; set; } // Eklenecek ürün ID'si
        }

        // Favorilerden ürün kaldırma isteği için kullanılan model
        public class RemoveFavoriteRequest
        {
            public int UserId { get; set; } // Kullanıcı ID'si
            public int ProductId { get; set; } // Kaldırılacak ürün ID'si
        }
    }
} 