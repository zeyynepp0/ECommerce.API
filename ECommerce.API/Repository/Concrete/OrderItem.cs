// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // OrderItem entity'si için
using ECommerce.API.Repository.Abstract; // IOrderItemRepository arayüzü için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Sipariş kalemlerine özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        // Veritabanı context'i
        private readonly MyDbContext _context;
        // OrderItemRepository constructor
        // <param name="context">Veritabanı context'i</param>
        public OrderItemRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
