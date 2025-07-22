using System;

namespace ECommerce.API.Entities.Concrete
{
    /// <summary>
    /// Kullanıcıya gönderilen bildirimleri temsil eden entity.
    /// </summary>
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
    }
} 