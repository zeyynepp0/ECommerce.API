using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repo;

        public AddressService(IAddressRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Address>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Address> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task AddAsync(Address address)
        {
            await _repo.AddAsync(address);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Address address)
        {
            _repo.Update(address);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item != null)
            {
                _repo.Delete(item);
                await _repo.SaveAsync();
            }
        }

        public async Task UpdateUserAddressAsync(int userId, Address address)
        {
            // Kullanıcının ilk adresini güncelle (örnek)
            var user = address.User;
            if (user == null) throw new Exception("User not found");
            var existing = user.Addresses.FirstOrDefault();
            if (existing == null) throw new Exception("Address not found");
            existing.Street = address.Street;
            existing.City = address.City;
            existing.State = address.State;
            existing.PostalCode = address.PostalCode;
            existing.Country = address.Country;
            _repo.Update(existing);
            await _repo.SaveAsync();
        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _repo.GetAddressesByUserIdAsync(userId);
        }
    }
}