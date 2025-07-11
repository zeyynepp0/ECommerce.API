using ECommerce.API.Data;
using ECommerce.API.DTO;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;
using Microsoft.EntityFrameworkCore;



namespace ECommerce.API.Services.Concrete
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _repo;
        private readonly MyDbContext _context;

        public FavoriteService(IFavoriteRepository repo, MyDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public IQueryable<Favorite> Favorites => _repo.Favorites;

        public async Task<List<FavoriteDto>> GetFavoritesByUserIdAsync(int userId)
        {
            var favorites = await _context.Favorites
        .Include(f => f.Product)
        .Where(f => f.UserId == userId)
        .Select(f => new FavoriteDto
        {
            Id = f.Id,
            ProductId = f.ProductId,
            ProductName = f.Product.Name,
            ProductImageUrl = f.Product.ImageUrl, 
            Price = f.Product.Price
        })
        .ToListAsync();

    return favorites;
        }

        public async Task<(bool success, string message)> AddToFavoritesAsync(int userId, int productId)
        {
            try
            {
                var existingFavorite = await _repo.GetFavoriteByUserAndProductAsync(userId, productId);
                if (existingFavorite != null)
                    return (false, "Ürün zaten favorilerde");

                var favorite = new Favorite
                {
                    UserId = userId,
                    ProductId = productId
                };

                await _repo.AddAsync(favorite);
                await _repo.SaveAsync();
                return (true, "Ürün favorilere eklendi");
            }
            catch
            {
                return (false, "Ekleme sýrasýnda bir hata oluþtu");
            }
        }




        public async Task<bool> RemoveFromFavoritesAsync(int userId, int productId)
        {
            try
            {
                var favorite = await _repo.GetFavoriteByUserAndProductAsync(userId, productId);
                if (favorite == null)
                    return false;

                _repo.Delete(favorite);
                await _repo.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsProductFavoritedByUserAsync(int userId, int productId)
        {
            return await _repo.IsProductFavoritedByUserAsync(userId, productId);
        }

       
    }
}
