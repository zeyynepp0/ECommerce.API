using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repository.Concrete
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        private readonly MyDbContext _context;

        public AddressRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
