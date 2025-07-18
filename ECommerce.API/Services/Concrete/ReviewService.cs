using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;
using ECommerce.API.DTO;

namespace ECommerce.API.Services.Concrete
{
   
    /// Ürün yorumlarıyla ilgili iş mantığını yöneten servis sınıfı.
   
    public class ReviewService : IReviewService
    {
       
        /// Yorum repository'si (veri erişim katmanı).
       
        private readonly IReviewRepository _repo;

       
        /// ReviewService constructor.
       
        /// <param name="repo">Yorum repository'si</param>
        public ReviewService(IReviewRepository repo)
        {
            _repo = repo;
        }

       
        /// Tüm yorumları getirir.
       
        public async Task<List<ReviewDto>> GetAllAsync(bool includeDeleted = false)
        {
            var reviews = await _repo.GetAllAsync();
            if (!includeDeleted)
                reviews = reviews.Where(r => !r.IsDeleted).ToList();
            return reviews.Select(r => new DTO.ReviewDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                Comment = r.IsDeleted ? "Bu yorum silinmiştir" : r.Comment,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt,
                ProductName = r.Product != null ? r.Product.Name : string.Empty,
                UserFullName = r.User != null ? r.User.FullName : string.Empty,
                LastModifiedBy = r.LastModifiedBy,
                LastModifiedAt = r.LastModifiedAt
            }).ToList();
        }

       
        /// Id'ye göre yorumu getirir. Yorum yoksa hata fırlatır.
       
        public async Task<ReviewDto> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return null;
            return new ReviewDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                Comment = r.Comment,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt,
                ProductName = r.Product != null ? r.Product.Name : string.Empty,
                UserFullName = r.User != null ? r.User.FullName : string.Empty
            };
        }

       
        /// Yeni yorum ekler.
       
        public async Task AddAsync(Review review)
        {
            await _repo.AddAsync(review);
            await _repo.SaveAsync();
        }

       
        /// Var olan yorumu günceller.
       
        public async Task UpdateAsync(UpdateReviewDto dto)
        {
            var review = await _repo.GetByIdAsync(dto.Id);
            if (review == null) return;
            review.Comment = dto.Content;
            review.Rating = dto.Rating;
            review.LastModifiedBy = dto.LastModifiedBy;
            review.LastModifiedAt = DateTime.UtcNow;
            _repo.Update(review);
            await _repo.SaveAsync();
        }

        public async Task SoftDeleteAsync(int id, string deletedBy)
        {
            var review = await _repo.GetByIdAsync(id);
            if (review != null && !review.IsDeleted)
            {
                review.IsDeleted = true;
                review.DeletedAt = DateTime.UtcNow;
                review.LastModifiedBy = deletedBy;
                review.LastModifiedAt = DateTime.UtcNow;
                _repo.Update(review);
                await _repo.SaveAsync();
            }
        }

        /// Id'ye göre yorumu siler (varsa).
       
        public async Task DeleteAsync(int id)
        {
            var review = await _repo.GetByIdAsync(id);
            if (review != null)
            {
                _repo.Delete(review);
                await _repo.SaveAsync();
            }
        }
        
       
        /// Belirli bir ürüne ait tüm yorumları getirir.
       
        public async Task<List<ReviewDto>> GetByProductIdAsync(int productId)
        {
            var reviews = await _repo.GetReviewsByProductIdAsync(productId);
            reviews = reviews.Where(r => !r.IsDeleted).ToList();
            return reviews.Select(r => new DTO.ReviewDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                Comment = r.IsDeleted ? "Bu yorum silinmiştir" : r.Comment,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt,
                ProductName = r.Product != null ? r.Product.Name : string.Empty,
                UserFullName = r.User != null ? r.User.FullName : string.Empty,
                LastModifiedBy = r.LastModifiedBy,
                LastModifiedAt = r.LastModifiedAt
            }).ToList();
        }
    }
}
