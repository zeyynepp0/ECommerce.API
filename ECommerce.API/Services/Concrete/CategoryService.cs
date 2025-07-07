using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Category>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Category> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task AddAsync(Category category)
        {
            await _repo.AddAsync(category);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _repo.Update(category);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category != null)
            {
                _repo.Delete(category);
                await _repo.SaveAsync();
            }
        }
    }
}
