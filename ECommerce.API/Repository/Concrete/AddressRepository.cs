// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // Address entity'si için
using ECommerce.API.Repository.Abstract; // IAddressRepository arayüzü için
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Adreslere özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        // Veritabanı context'i
        private readonly MyDbContext _context;

        // AddressRepository constructor
        // <param name="context">Veritabanı context'i</param>
        public AddressRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        // Belirli bir kullanıcıya ait adresleri getirir
        public async Task<List<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
