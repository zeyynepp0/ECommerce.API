using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Repository.Abstract
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<List<Review>> GetReviewsByProductIdAsync(int productId);
    }
}
