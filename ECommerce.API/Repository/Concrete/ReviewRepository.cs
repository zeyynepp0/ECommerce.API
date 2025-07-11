using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repository.Concrete
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly MyDbContext _context;

        public ReviewRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

    }
}
