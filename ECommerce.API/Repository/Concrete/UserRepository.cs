// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // User entity'si için
using ECommerce.API.Repository.Abstract; // IUserRepository arayüzü için
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Kullanıcıya özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class UserRepository : Repository<User>, IUserRepository
    {
        // Veritabanı context'i
        private readonly MyDbContext _context;

        // UserRepository constructor
        // <param name="context">Veritabanı context'i</param>
        public UserRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        // E-posta adresine göre kullanıcıyı getirir
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Toplam kullanıcı sayısını döndürür
        public async Task<int> CountAsync()
        {
            return await _context.Users.CountAsync();
        }

        // Belirli bir tarih aralığında doğan kullanıcıların sayısını döndürür
        public async Task<int> CountByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _context.Users.CountAsync(u => u.BirthDate >= start && u.BirthDate <= end);
        }
    }
}
