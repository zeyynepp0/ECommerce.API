// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Kullanıcı verilerini taşımak için kullanılan DTO sınıfı
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public DateTime? BirthDate { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
    }
} 