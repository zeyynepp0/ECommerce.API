// Address entity'sini içeren namespace'i içeri aktarır
using ECommerce.API.Entities.Concrete;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // Address entity'si için repository arayüzü. IRepository<Address> arayüzünden türetilir.
    public interface IAddressRepository : IRepository<Address>
    {
        // Belirli bir kullanıcıya ait adresleri asenkron olarak getirir
        Task<List<Address>> GetAddressesByUserIdAsync(int userId);
    }
}
