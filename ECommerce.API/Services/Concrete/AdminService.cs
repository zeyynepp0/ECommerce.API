using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repo;

        public AdminService(IAdminRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Admin>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Admin> GetByIdAsync(int id)
        {
            var admin = await _repo.GetByIdAsync(id);
            if (admin == null) throw new Exception("Admin not found");
            return admin;
        }

        public async Task AddAsync(Admin admin)
        {
            var exists = await _repo.FindAsync(a => a.AdminName == admin.AdminName);
            if (exists.Any()) throw new Exception("Admin already exists");
            await _repo.AddAsync(admin);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Admin admin)
        {
            _repo.Update(admin);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var admin = await _repo.GetByIdAsync(id);
            if (admin != null)
            {
                _repo.Delete(admin);
                await _repo.SaveAsync();
            }
        }
    }
}
