// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Kullanıcı verilerini taşımak için kullanılan DTO sınıfı
    public class UserDto
    {
        public int Id { get; set; } // Kullanıcı ID'si
        public string FirstName { get; set; } // Kullanıcının adı
        public string LastName { get; set; } // Kullanıcının soyadı
        public string Email { get; set; } // Kullanıcının e-posta adresi
        public string Phone { get; set; } // Kullanıcının telefon numarası
        public string Role { get; set; } // Kullanıcının rolü (Admin, User vb.)
        public bool EmailConfirmed { get; set; } // E-posta onay durumu
        public DateTime BirthDate { get; set; } // Doğum tarihi
        public int OrderCount { get; set; } // Kullanıcının toplam sipariş sayısı
        public decimal TotalSpent { get; set; } // Kullanıcının toplam harcaması
    }
} 