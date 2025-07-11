using ECommerce.API.Entities.Concrete;
using ECommerce.API.DTO;

namespace ECommerce.API.Services.Abstract
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task AddAsync(Order order);
        Task<Order> CreateOrderAsync(OrderDto orderDto);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task UpdateStatusAsync(int orderId, string status);
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task ApproveOrderAsync(int orderId);
        Task RejectOrderAsync(int orderId);
        Task<object> GetEarningsReportAsync(string period);
        Task<List<Order>> GetByUserIdAsync(int userId);
    }
}
