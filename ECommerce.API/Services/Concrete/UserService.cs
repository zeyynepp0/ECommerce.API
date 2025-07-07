using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<User>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<User> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<User> GetByEmailAsync(string email) => await _repo.GetByEmailAsync(email);

        public async Task AddAsync(User user)
        {
            await _repo.AddAsync(user);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _repo.Update(user);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user != null)
            {
                _repo.Delete(user);
                await _repo.SaveAsync();
            }
        }
    }
}
