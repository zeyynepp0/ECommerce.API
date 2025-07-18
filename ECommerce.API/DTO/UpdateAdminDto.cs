// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Admin güncelleme işlemleri için kullanılan DTO sınıfı
    public class UpdateAdminDto
    {
        public int Id { get; set; } // Admin ID'si
        public string Name { get; set; } // Admin adı
        public string Email { get; set; } // Admin e-posta adresi
        public string Password { get; set; } // Admin şifresi
    }
} 