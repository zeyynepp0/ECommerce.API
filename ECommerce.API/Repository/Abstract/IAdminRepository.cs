// ECommerce.API.Entities.Concrete namespace'inden gerekli sınıfları içeri aktarır
using ECommerce.API.Entities.Concrete;
// ECommerce.API.Data namespace'inden gerekli sınıfları içeri aktarır
using ECommerce.API.Data;
// Repository arayüzlerini içeren namespace'i içeri aktarır
using ECommerce.API.Repository.Abstract;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // Admin entity'si için repository arayüzü. IRepository<Admin> arayüzünden türetilir.
    public interface IAdminRepository : IRepository<Admin>
    {
        // Admin nesnesini asenkron olarak günceller. Başarı durumunu bool olarak döner.
        Task<bool> UpdateAsync(Admin admin);
        Task<Admin> GetByEmailAndPasswordAsync(string email, string password);
    }
}
