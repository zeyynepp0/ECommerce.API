using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;

namespace ECommerce.API.Repository.Concrete
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly MyDbContext _context;

        public PaymentRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
