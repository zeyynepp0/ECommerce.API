using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Product>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) throw new Exception("Product not found");
            return product;
        }

        public async Task<List<Product>> GetFilteredAsync(string keyword) => await _repo.GetFilteredAsync(keyword);

        public async Task AddAsync(Product product)
        {
            var exists = await _repo.FindAsync(p => p.Name == product.Name);
            if (exists.Any()) throw new Exception("Product already exists");

            await _repo.AddAsync(product);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _repo.Update(product);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product != null)
            {
                _repo.Delete(product);
                await _repo.SaveAsync();
            }
        }
    }
}
