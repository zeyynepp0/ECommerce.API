using ECommerce.API.Data;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Repository.Concrete
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<List<Notification>> GetByUserIdAsync(int userId)
        {
            return await Context.Notifications.Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await Context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
} 