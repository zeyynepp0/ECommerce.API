// Kullanıcı servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.DTO;
using ECommerce.API.Entities.Concrete; // Kullanıcı varlık sınıfı

namespace ECommerce.API.Services.Abstract
{
    // Kullanıcı işlemleri için servis arayüzü
    public interface IUserService
    {
        Task<List<User>> GetAllAsync(); // Tüm kullanıcıları asenkron olarak getirir
        Task<User> GetByIdAsync(int id); // Id'ye göre kullanıcıyı asenkron olarak getirir
        Task<User> GetByEmailAsync(string email); // E-posta ile kullanıcıyı getirir (asenkron)
        Task AddAsync(User user); // Yeni kullanıcı ekler (asenkron)
        Task UpdateAsync(User user); // Kullanıcıyı günceller (asenkron)
        Task DeleteAsync(int id); // Id'ye göre kullanıcıyı siler (asenkron)
        Task<User?> AuthenticateAsync(string email, string password); // Kullanıcı girişini doğrular (asenkron)
        Task UpdateAsync(UpdateUserDto dto);
        Task SetActiveAsync(int userId, bool isActive);

        // Şifremi unuttum (token üretir ve kullanıcıya yollar)
        Task ForgotPasswordAsync(string email);
        // Şifre sıfırlama (token ve yeni şifre ile)
        Task ResetPasswordAsync(string token, string newPassword);
    }
}
