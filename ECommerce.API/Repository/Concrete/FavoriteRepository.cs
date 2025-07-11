using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repository.Concrete
{
    public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
    {
        private readonly MyDbContext _context;

        public FavoriteRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Favorite> Favorites => _context.Favorites;

        public async Task<List<Favorite>> GetFavoritesByUserIdAsync(int userId)
        {
            return await _context.Favorites
        .Include(f => f.Product)
        .Where(f => f.UserId == userId)
        .ToListAsync();
        }

        public async Task<Favorite> GetFavoriteByUserAndProductAsync(int userId, int productId)
        {
            return await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
        }

        public async Task<bool> IsProductFavoritedByUserAsync(int userId, int productId)
        {
            return await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId);
        }
    }
} 