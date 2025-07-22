using ECommerce.API.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.API.Repository.Abstract
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<List<Notification>> GetByUserIdAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
    }
} 