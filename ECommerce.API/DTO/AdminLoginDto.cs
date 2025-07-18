// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Admin giriş işlemleri için kullanılan DTO sınıfı
    public class AdminLoginDto
    {
        public string Email { get; set; } // Admin e-posta adresi
        public string Password { get; set; } // Admin şifresi
    }
} 