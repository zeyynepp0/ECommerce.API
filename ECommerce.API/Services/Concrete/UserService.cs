using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<User>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<User> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<User> GetByEmailAsync(string email) => await _repo.GetByEmailAsync(email);

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
            await _repo.AddAsync(user);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _repo.Update(user);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user != null)
            {
                _repo.Delete(user);
                await _repo.SaveAsync();
            }
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = (await _repo.FindAsync(u => u.Email == email)).FirstOrDefault();
            if (user == null) return null;
            // BCrypt hash kontrolü
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
            return user;
        }

        public async Task UpdateRoleAsync(int userId, string role)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");
            user.Role = Enum.Parse<UserRole>(role);
            _repo.Update(user);
            await _repo.SaveAsync();
        }
    }
}
