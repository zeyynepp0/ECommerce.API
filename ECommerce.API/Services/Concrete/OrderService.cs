using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;
using ECommerce.API.DTO;
using ECommerce.API.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly MyDbContext _context;

        public OrderService(IOrderRepository repo, MyDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Order> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Address)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _repo.AddAsync(order);
            await _repo.SaveAsync();
        }

        public async Task<Order> CreateOrderAsync(OrderDto orderDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Sipariş oluştur
                var order = new Order
                {
                    UserId = orderDto.UserId,
                    AddressId = orderDto.AddressId,
                    ShippingCompany = orderDto.ShippingCompany,
                    PaymentMethod = orderDto.PaymentMethod,
                    TotalAmount = orderDto.TotalAmount,
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                // Sipariş kalemlerini oluştur
                foreach (var item in orderDto.OrderItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    await _context.OrderItems.AddAsync(orderItem);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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

        public async Task UpdateStatusAsync(int orderId, string status)
        {
            var order = await _repo.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");
            order.Status = status;
            _repo.Update(order);
            await _repo.SaveAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _repo.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");
            order.Status = status;
            _repo.Update(order);
            await _repo.SaveAsync();
        }

        public async Task ApproveOrderAsync(int orderId)
        {
            var order = await _repo.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");
            order.Status = "Approved";
            _repo.Update(order);
            await _repo.SaveAsync();
        }

        public async Task RejectOrderAsync(int orderId)
        {
            var order = await _repo.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");
            order.Status = "Rejected";
            _repo.Update(order);
            await _repo.SaveAsync();
        }

        public async Task<object> GetEarningsReportAsync(string period)
        {
            var orders = await _repo.GetAllAsync();
            DateTime start = DateTime.Now;
            if (period == "weekly") start = DateTime.Now.AddDays(-7);
            else if (period == "monthly") start = DateTime.Now.AddMonths(-1);
            else if (period == "yearly") start = DateTime.Now.AddYears(-1);
            var filtered = orders.Where(o => o.OrderDate >= start && o.Status == "Approved");
            var total = filtered.Sum(o => o.TotalAmount);
            return new { period, total };
        }

        public async Task<List<Order>> GetByUserIdAsync(int userId)
        {
            return (await _repo.GetAllAsync()).Where(o => o.UserId == userId).ToList();
        }
    }
}
