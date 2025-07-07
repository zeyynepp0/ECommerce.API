using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Repository.Abstract
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        void Delete(Order order);
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task SaveAsync();
        void Update(Order order);
    }
}
