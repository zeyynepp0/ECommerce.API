using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.DTO
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public DateTime? BirthDate { get; set; }

        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı")]
        public string? Password { get; set; } // Plain password (opsiyonel)

        public string? PasswordHash { get; set; } // Hash şifre (opsiyonel)

        public bool IsActive { get; set; } = true;
    }
}
