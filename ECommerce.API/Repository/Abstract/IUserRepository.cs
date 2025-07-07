using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Repository.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
