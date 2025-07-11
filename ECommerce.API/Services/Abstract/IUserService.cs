using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Services.Abstract
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<User?> AuthenticateAsync(string email, string password);
        Task UpdateRoleAsync(int userId, string role);
    }
}
