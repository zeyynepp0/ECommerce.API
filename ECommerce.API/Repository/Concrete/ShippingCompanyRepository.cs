using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Repository.Concrete
{
    /// <summary>
    /// Kargo firması repository implementasyonu.
    /// </summary>
    public class ShippingCompanyRepository : Repository<ShippingCompany>, IShippingCompanyRepository
    {
        private readonly MyDbContext _context;
        public ShippingCompanyRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Sadece aktif kargo firmalarını getirir.
        /// </summary>
        public async Task<List<ShippingCompany>> GetActiveShippingCompaniesAsync()
        {
            return await _context.ShippingCompanies.Where(x => x.IsActive).ToListAsync();
        }
    }
} 