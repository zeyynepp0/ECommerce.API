// User entity'sini içeren namespace'i içeri aktarır
using ECommerce.API.Entities.Concrete;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // User entity'si için repository arayüzü. IRepository<User> arayüzünden türetilir.
    public interface IUserRepository : IRepository<User>
    {
        // E-posta adresine göre kullanıcıyı asenkron olarak getirir
        Task<User> GetByEmailAsync(string email);
        Task<int> CountAsync();
        Task<int> CountByDateRangeAsync(DateTime start, DateTime end);
    }
}
