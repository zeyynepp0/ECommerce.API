using ECommerce.API.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.API.Services.Abstract
{
    public interface IAdminService
    {
        Task<List<Admin>> GetAllAsync();
        Task<Admin> GetByIdAsync(int id);
        Task AddAsync(Admin admin);
        Task UpdateAsync(Admin admin);
        Task DeleteAsync(int id);
    }
}
