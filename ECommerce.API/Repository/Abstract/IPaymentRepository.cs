// Payment entity'sini içeren namespace'i içeri aktarır
using ECommerce.API.Entities.Concrete;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // Payment entity'si için repository arayüzü. IRepository<Payment> arayüzünden türetilir.
    public interface IPaymentRepository : IRepository<Payment>
    {
        // Şu anda ek bir metot yok, sadece temel işlemler kullanılabilir
    }
}
