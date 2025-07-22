using ECommerce.API.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.API.Services.Abstract
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetByUserIdAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task AddAsync(NotificationDto dto);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(int userId);
    }
} 