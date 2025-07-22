// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // Order entity'si için
using ECommerce.API.Repository.Abstract; // IOrderRepository arayüzü için
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Siparişlere özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        // Veritabanı context'ini tutan private alan
        private readonly MyDbContext _context;

        // OrderRepository sınıfının yapıcı metodu
        // context: Veritabanı context'i dışarıdan alınır
        public OrderRepository(MyDbContext context) : base(context)
        {
            _context = context; // context alanına atanır
        }

        // Belirli bir kullanıcıya ait siparişleri, sipariş kalemleriyle birlikte asenkron olarak getirir
        // userId: Kullanıcının benzersiz kimliği
        // Dönüş: Kullanıcıya ait siparişlerin listesi
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            // Orders tablosunda userId'ye göre filtreleme yapılır, OrderItems ile birlikte çekilir
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        // Siparişin durumunu günceller
        // orderId: Güncellenecek siparişin kimliği
        // status: Yeni durum bilgisi
        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            // İlgili sipariş bulunur
            var order = await _context.Orders.FindAsync(orderId);
            // Sipariş bulunamazsa hata fırlatılır
            if (order == null) throw new Exception("Order not found");
            // Siparişin durumu güncellenir
            order.Status = status;
            // Güncellenen sipariş context'e bildirilir
            _context.Orders.Update(order);
            // Değişiklikler veritabanına kaydedilir
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Category)
                .Include(o => o.Address)
                .Include(o => o.User)
                .Include(o => o.ShippingCompany)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
