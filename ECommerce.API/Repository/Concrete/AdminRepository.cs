using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;

namespace ECommerce.API.Repository.Concrete
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly MyDbContext _context;

        public AdminRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
