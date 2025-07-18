// Review entity'sini içeren namespace'i içeri aktarır
using ECommerce.API.Entities.Concrete;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // Review entity'si için repository arayüzü. IRepository<Review> arayüzünden türetilir.
    public interface IReviewRepository : IRepository<Review>
    {
        // Belirli bir ürüne ait tüm yorumları asenkron olarak getirir
        Task<List<Review>> GetReviewsByProductIdAsync(int productId);
    }
}
