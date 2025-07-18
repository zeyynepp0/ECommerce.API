// Adres servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Adres varlık sınıfı

namespace ECommerce.API.Services.Abstract
{
    // Adres işlemleri için servis arayüzü
    public interface IAddressService
    {
        Task<List<Address>> GetAllAsync(); // Tüm adresleri asenkron olarak getirir
        Task<Address> GetByIdAsync(int id); // Id'ye göre adresi asenkron olarak getirir
        Task AddAsync(Address address); // Yeni adres ekler (asenkron)
        Task UpdateAsync(Address address); // Adresi günceller (asenkron)
        Task DeleteAsync(int id); // Id'ye göre adresi siler (asenkron)
        Task<List<Address>> GetAddressesByUserIdAsync(int userId); // Kullanıcıya ait adresleri getirir (asenkron)
    }
}
