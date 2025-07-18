// Category entity'sini içeren namespace'i içeri aktarır
using ECommerce.API.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // Category entity'si için repository arayüzü. IRepository<Category> arayüzünden türetilir.
    public interface ICategoryRepository : IRepository<Category>
    {
        // Şu anda ek bir metot yok, sadece temel işlemler kullanılabilir
        Task<int> CountAsync();
    }
}
