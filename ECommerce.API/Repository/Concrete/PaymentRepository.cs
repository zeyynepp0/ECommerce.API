// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Entities.Concrete; // Payment entity'si için
using ECommerce.API.Repository.Abstract; // IPaymentRepository arayüzü için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Ödeme işlemlerine özel veri erişim işlemlerini gerçekleştiren repository sınıfı
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        // Veritabanı context'i
        private readonly MyDbContext _context;

        // PaymentRepository constructor
        // <param name="context">Veritabanı context'i</param>
        public PaymentRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
