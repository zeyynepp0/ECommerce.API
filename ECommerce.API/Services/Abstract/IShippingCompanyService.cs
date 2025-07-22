using ECommerce.API.DTO;
using ECommerce.API.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.API.Services.Abstract
{
    /// <summary>
    /// Kargo firması servis arayüzü.
    /// </summary>
    public interface IShippingCompanyService
    {
        Task<List<ShippingCompany>> GetAllAsync();
        Task<List<ShippingCompany>> GetActiveAsync();
        Task<ShippingCompany> GetByIdAsync(int id);
        Task AddAsync(ShippingCompanyDto dto);
        Task UpdateAsync(int id, ShippingCompanyDto dto);
        Task<bool> DeleteAsync(int id);
    }
} 