using ECommerce.API.DTO;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Services.Concrete
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        public NotificationService(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<NotificationDto>> GetByUserIdAsync(int userId)
        {
            var notifications = await _repo.GetByUserIdAsync(userId);
            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            }).ToList();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _repo.GetUnreadCountAsync(userId);
        }

        public async Task AddAsync(NotificationDto dto)
        {
            var entity = new Notification
            {
                UserId = dto.UserId,
                Message = dto.Message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notifications = await _repo.FindAsync(n => n.Id == notificationId);
            var notification = notifications.FirstOrDefault();
            if (notification != null)
            {
                notification.IsRead = true;
                _repo.Update(notification);
                await _repo.SaveAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifications = await _repo.GetByUserIdAsync(userId);
            foreach (var n in notifications.Where(n => !n.IsRead))
            {
                n.IsRead = true;
                _repo.Update(n);
            }
            await _repo.SaveAsync();
        }
    }
} 