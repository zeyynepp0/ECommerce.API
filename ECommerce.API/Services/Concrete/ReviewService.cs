using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repo;

        public ReviewService(IReviewRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Review>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Review> GetByIdAsync(int id)
        {
            var review = await _repo.GetByIdAsync(id);
            if (review == null) throw new Exception("Review not found");
            return review;
        }

        public async Task AddAsync(Review review)
        {
            await _repo.AddAsync(review);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Review review)
        {
            _repo.Update(review);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _repo.GetByIdAsync(id);
            if (review != null)
            {
                _repo.Delete(review);
                await _repo.SaveAsync();
            }
        }
        
        public async Task<List<Review>> GetByProductIdAsync(int productId)
        {
            return await _repo.GetReviewsByProductIdAsync(productId);
        }
    }

}
