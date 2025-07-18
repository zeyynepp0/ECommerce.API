// Sipariş servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Sipariş varlık sınıfı
using ECommerce.API.DTO; // DTO sınıfları

namespace ECommerce.API.Services.Abstract
{
    // Sipariş işlemleri için servis arayüzü
    public interface IOrderService
    {
        Task<List<Order>> GetAllAsync(); // Tüm siparişleri asenkron olarak getirir
        Task<Order> GetByIdAsync(int id); // Id'ye göre siparişi asenkron olarak getirir
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId); // Kullanıcıya ait siparişleri getirir (asenkron)
        Task AddAsync(Order order); // Yeni sipariş ekler (asenkron)
        Task<List<OrderDto>> GetByUserIdAsync(int userId); // Kullanıcıya ait siparişleri getirir (asenkron)
        Task<Order> CreateOrderAsync(OrderDto orderDto); // Yeni sipariş oluşturur (asenkron)
        Task UpdateOrderStatusAsync(int orderId, string status); // Sipariş durumunu günceller (asenkron)
        Task UpdateAsync(Order order); // Siparişi günceller (asenkron)
        Task DeleteAsync(int id); // Siparişi siler (asenkron)
    }
}
