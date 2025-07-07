using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Order>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Order> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task AddAsync(Order order)
        {
            await _repo.AddAsync(order);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _repo.Update(order);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _repo.GetByIdAsync(id);
            if (order != null)
            {
                _repo.Delete(order);
                await _repo.SaveAsync();
            }
        }
    }
}
