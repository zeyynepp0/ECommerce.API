// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // Admin entity'si için
using ECommerce.API.Repository.Abstract; // IAdminRepository arayüzü için
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Adminlere özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        // AdminRepository constructor
        // <param name="context">Veritabanı context'i</param>
        public AdminRepository(MyDbContext context) : base(context)
        {
        }

        // E-posta ve şifreye göre admin bilgisini getirir
        public async Task<Admin> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await Context.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.User.Email == email && a.User.PasswordHash == password);
        }

        // Admin bilgisini günceller
        public async Task<bool> UpdateAsync(Admin admin)
        {
            Context.Admins.Update(admin);
            return await Context.SaveChangesAsync() > 0;
        }
    }
}
