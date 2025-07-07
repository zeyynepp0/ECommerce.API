using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;

namespace ECommerce.API.Repository.Concrete
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly MyDbContext _context;
        public OrderItemRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
