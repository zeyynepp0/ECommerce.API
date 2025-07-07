using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;

namespace ECommerce.API.Repository.Concrete
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private readonly MyDbContext _context;
        public CartItemRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
