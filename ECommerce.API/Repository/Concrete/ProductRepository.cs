using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repository.Concrete
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly MyDbContext _context;

        public ProductRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsWithCategoryAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<List<Product>> GetFilteredAsync(string keyword)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
                .ToListAsync();
        }
        public async Task<List<Product>> GetByCategoryIdAsync(int categoryId, int excludeProductId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId && p.Id != excludeProductId)
                .ToListAsync();
        }


    }
}
