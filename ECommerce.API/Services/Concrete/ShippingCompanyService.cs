using ECommerce.API.DTO;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.API.Services.Concrete
{
    /// <summary>
    /// Kargo firması servis implementasyonu.
    /// </summary>
    public class ShippingCompanyService : IShippingCompanyService
    {
        private readonly IShippingCompanyRepository _repo;
        // İleride Order ile ilişki kurulursa burada IOrderRepository de inject edilebilir.

        public ShippingCompanyService(IShippingCompanyRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ShippingCompany>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<List<ShippingCompany>> GetActiveAsync()
        {
            return await _repo.GetActiveShippingCompaniesAsync();
        }

        public async Task<ShippingCompany> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(ShippingCompanyDto dto)
        {
            var entity = new ShippingCompany
            {
                Name = dto.Name,
                Price = dto.Price,
                IsActive = dto.IsActive,
                FreeShippingLimit = dto.FreeShippingLimit, // Ücretsiz kargo limiti
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(int id, ShippingCompanyDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Kargo firması bulunamadı.");
            entity.Name = dto.Name;
            entity.Price = dto.Price;
            entity.IsActive = dto.IsActive;
            entity.FreeShippingLimit = dto.FreeShippingLimit; // Ücretsiz kargo limiti
            entity.UpdatedAt = DateTime.UtcNow;
            _repo.Update(entity);
            await _repo.SaveAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            // İleride Order ile ilişki kurulursa burada kullanımda kontrolü yapılacak.
            _repo.Delete(entity);
            await _repo.SaveAsync();
            return true;
        }
    }
} 