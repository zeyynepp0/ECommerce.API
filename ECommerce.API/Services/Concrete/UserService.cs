using ECommerce.API.DTO;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;
using BCrypt.Net;
using ECommerce.API.Utilities;

namespace ECommerce.API.Services.Concrete
{
   
    /// Kullanıcılarla ilgili iş mantığını yöneten servis sınıfı.
   
    public class UserService : IUserService
    {
       
        /// Kullanıcı repository'si (veri erişim katmanı).
       
        private readonly IUserRepository _repo;
        private readonly EmailService _emailService;

        /// UserService constructor.
        public UserService(IUserRepository repo, EmailService emailService)
        {
            _repo = repo;
            _emailService = emailService;
        }

       
        /// Tüm kullanıcıları getirir.
       
        public async Task<List<User>> GetAllAsync() => await _repo.GetAllAsync();

       
        /// Id'ye göre kullanıcıyı getirir.
       
        public async Task<User> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

       
        /// E-posta adresine göre kullanıcıyı getirir.
       
        public async Task<User> GetByEmailAsync(string email) => await _repo.GetByEmailAsync(email);

       
        /// Yeni kullanıcı ekler. E-posta benzersizliği ve doğum tarihi kontrolü yapar, şifreyi hash'ler.
       
        public async Task AddAsync(User user)
        {
            // E-posta benzersizliği kontrolü
            var existing = await _repo.GetByEmailAsync(user.Email);
            if (existing != null)
                throw new Exception("Bu e-posta ile zaten bir kullanıcı var.");
            // Doğum tarihi kontrolü
            if (user.BirthDate > DateTime.Now)
                throw new Exception("Doğum tarihi bugünden ileri olamaz.");
            // Şifre hash'leme
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            // isActive default true veya gelen değeri kullan
            user.IsActive = user.IsActive;
            await _repo.AddAsync(user);
            await _repo.SaveAsync();
        }


        /// Var olan kullanıcıyı günceller.

        public async Task UpdateAsync(User user)
        {
            _repo.Update(user);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(UpdateUserDto dto)
        {
            var user = await _repo.GetByIdAsync(dto.Id);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;

            // string to enum dönüşümü
            user.Role = Enum.Parse<UserRole>(dto.Role);

            // Nullable tarih dönüşümü
            user.BirthDate = dto.BirthDate ?? user.BirthDate;

            // Şifre güncellemesi
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }
            else if (!string.IsNullOrWhiteSpace(dto.PasswordHash))
            {
                user.PasswordHash = dto.PasswordHash;
            }
            // Aktiflik güncellemesi
            user.IsActive = dto.IsActive;

            _repo.Update(user);
            await _repo.SaveAsync();
        }


        /// Id'ye göre kullanıcıyı siler (varsa).

        public async Task DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user != null)
            {
                _repo.Delete(user);
                await _repo.SaveAsync();
            }
        }

       
        /// Kullanıcıyı e-posta ve şifre ile doğrular. Şifre hash kontrolü yapar.
       
        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = (await _repo.FindAsync(u => u.Email.ToLower() == email.ToLower())).FirstOrDefault();
            if (user == null) return null;
            // BCrypt hash kontrolü
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
            // E-posta doğrulama kontrolü
            if (!user.EmailConfirmed)
            {
                // Doğrulama tokenı üret
                var token = Guid.NewGuid().ToString("N");
                user.EmailVerificationToken = token;
                user.EmailVerificationTokenExpires = DateTime.UtcNow.AddHours(1);
                _repo.Update(user);
                await _repo.SaveAsync();
                await _emailService.SendVerificationEmailAsync(user.Email, token);
                throw new Exception("E-posta adresiniz doğrulanmamış. Doğrulama linki e-posta adresinize gönderildi.");
            }
            return user;
        }

       
        /// Kullanıcının rolünü günceller.
       
        public async Task UpdateRoleAsync(int userId, string role)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");
            user.Role = Enum.Parse<UserRole>(role);
            _repo.Update(user);
            await _repo.SaveAsync();
        }

        // Kullanıcıyı aktif/pasif yapar
        public async Task SetActiveAsync(int userId, bool isActive)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null) throw new Exception("Kullanıcı bulunamadı");
            user.IsActive = isActive;
            _repo.Update(user);
            await _repo.SaveAsync();
        }

        // Şifremi unuttum: Token üret ve kullanıcıya kaydet
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("Bu e-posta ile kayıtlı kullanıcı bulunamadı.");
            // Token üret
            var token = Guid.NewGuid().ToString("N");
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1); // 1 saat geçerli
            _repo.Update(user);
            await _repo.SaveAsync();
            string resetUrl = $"http://localhost:5173/reset-password?token={token}";
            string body = $"Şifrenizi sıfırlamak için <a href='{resetUrl}'>buraya tıklayın</a>.<br/>Veya bu linki tarayıcınıza yapıştırın: {resetUrl}";
            await _emailService.SendEmailAsync(user.Email, "Şifre Sıfırlama", body);
        }

        // Şifre sıfırlama: Token ve yeni şifre ile
        public async Task ResetPasswordAsync(string token, string newPassword)
        {
            var users = await _repo.FindAsync(u => u.PasswordResetToken == token && u.PasswordResetTokenExpires > DateTime.UtcNow);
            var user = users.FirstOrDefault();
            if (user == null)
                throw new Exception("Geçersiz veya süresi dolmuş token.");
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;
            _repo.Update(user);
            await _repo.SaveAsync();
        }
    }
}
