// Order entity'sini içeren namespace'i içeri aktarır
using ECommerce.API.Entities.Concrete;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // Order entity'si için repository arayüzü
    public interface IOrderRepository
    {
        // Tüm siparişleri asenkron olarak getirir
        Task<List<Order>> GetAllAsync();
        // Id'ye göre tek bir siparişi asenkron olarak getirir
        Task<Order> GetByIdAsync(int id);
        // Belirli bir kullanıcıya ait siparişleri asenkron olarak getirir
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        // Yeni bir siparişi asenkron olarak ekler
        Task AddAsync(Order order);
        // Var olan bir siparişi günceller
        void Update(Order order);
        // Var olan bir siparişi siler
        void Delete(Order order);
        // Yapılan değişiklikleri veritabanına kaydeder (asenkron)
        Task SaveAsync();
        // Siparişin durumunu asenkron olarak günceller
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}
