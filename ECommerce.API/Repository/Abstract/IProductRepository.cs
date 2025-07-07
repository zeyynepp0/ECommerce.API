using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Repository.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetProductsWithCategoryAsync();
        Task<List<Product>> GetFilteredAsync(string keyword);
    }
}
