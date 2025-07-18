using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Services.Concrete
{
   
    /// Sipariş kalemleriyle ilgili iş mantığını yöneten servis sınıfı.
   
    public class OrderItemService : IOrderItemService
    {
       
        /// Veritabanı context'i.
       
        private readonly MyDbContext _context;

       
        /// OrderItemService constructor.
       
        // <param name="context">Veritabanı context'i</param>
        public OrderItemService(MyDbContext context)
        {
            _context = context;
        }

       
        /// Tüm sipariş kalemlerini getirir.
       
        public async Task<List<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems.ToListAsync();
        }

       
        /// Id'ye göre sipariş kalemini getirir.
       
        public async Task<OrderItem> GetByIdAsync(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

       
        /// Yeni sipariş kalemi ekler.
       
        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

       
        /// Var olan sipariş kalemini günceller.
       
        public async Task UpdateAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }

       
        /// Id'ye göre sipariş kalemini siler (varsa).
       
        public async Task DeleteAsync(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item != null)
            {
                _context.OrderItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
