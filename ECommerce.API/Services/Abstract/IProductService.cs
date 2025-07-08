using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Services.Abstract
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<List<Product>> GetFilteredAsync(string keyword);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> GetByCategoryIdAsync(int categoryId ,int excludeProductId);

    }
}
