using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Repository.Abstract
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<List<Address>> GetAddressesByUserIdAsync(int userId);
    }
}
