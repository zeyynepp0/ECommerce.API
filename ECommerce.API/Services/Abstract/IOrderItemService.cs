// Sipariş kalemi servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Sipariş kalemi varlık sınıfı

namespace ECommerce.API.Services.Abstract
{
    // Sipariş kalemi işlemleri için servis arayüzü
    public interface IOrderItemService
    {
        Task<List<OrderItem>> GetAllAsync(); // Tüm sipariş kalemlerini asenkron olarak getirir
        Task<OrderItem> GetByIdAsync(int id); // Id'ye göre sipariş kalemini asenkron olarak getirir
        Task AddAsync(OrderItem orderItem); // Yeni sipariş kalemi ekler (asenkron)
        Task UpdateAsync(OrderItem orderItem); // Sipariş kalemini günceller (asenkron)
        Task DeleteAsync(int id); // Sipariş kalemini siler (asenkron)
    }
}
