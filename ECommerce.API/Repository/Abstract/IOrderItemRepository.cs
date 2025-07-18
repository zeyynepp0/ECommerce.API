// OrderItem entity'sini içeren namespace'i içeri aktarır
using ECommerce.API.Entities.Concrete;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // OrderItem entity'si için repository arayüzü. IRepository<OrderItem> arayüzünden türetilir.
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        // Şu anda ek bir metot yok, sadece temel işlemler kullanılabilir
    }
}