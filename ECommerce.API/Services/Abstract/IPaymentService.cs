// Ödeme servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Ödeme varlık sınıfı
using System.Collections.Generic; // Liste işlemleri için
using System.Threading.Tasks; // Asenkron işlemler için

namespace ECommerce.API.Services.Abstract
{
    // Ödeme işlemleri için servis arayüzü
    public interface IPaymentService
    {
        Task<List<Payment>> GetAllAsync(); // Tüm ödemeleri asenkron olarak getirir
        Task<Payment> GetByIdAsync(int id); // Id'ye göre ödemeyi asenkron olarak getirir
        Task AddAsync(Payment payment); // Yeni ödeme ekler (asenkron)
        Task UpdateAsync(Payment payment); // Ödemeyi günceller (asenkron)
        Task DeleteAsync(int id); // Ödemeyi siler (asenkron)
    }
}
