using ECommerce.API.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.API.Repository.Abstract
{
    /// <summary>
    /// Kargo firması repository arayüzü.
    /// </summary>
    public interface IShippingCompanyRepository : IRepository<ShippingCompany>
    {
        /// <summary>
        /// Sadece aktif kargo firmalarını getirir.
        /// </summary>
        Task<List<ShippingCompany>> GetActiveShippingCompaniesAsync();
    }
} 