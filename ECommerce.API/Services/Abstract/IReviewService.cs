// Yorum servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Yorum varlık sınıfı
using System.Collections.Generic; // Liste işlemleri için
using System.Threading.Tasks; // Asenkron işlemler için
using ECommerce.API.DTO; // Yorum DTO sınıfı

namespace ECommerce.API.Services.Abstract
{
    // Yorum işlemleri için servis arayüzü
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetAllAsync(bool includeDeleted = false); // Tüm yorumları asenkron olarak getirir
        Task<ReviewDto> GetByIdAsync(int id); // Id'ye göre yorumu asenkron olarak getirir
        Task AddAsync(Review review); // Yeni yorum ekler (asenkron)
        Task UpdateAsync(UpdateReviewDto dto); // Yorumu günceller (asenkron)
        Task DeleteAsync(int id); // Id'ye göre yorumu siler (asenkron)
        Task SoftDeleteAsync(int id, string deletedBy); // Yorumu silindi olarak işaretler (asenkron)
        Task<List<ReviewDto>> GetByProductIdAsync(int productId); // Ürüne ait yorumları getirir (asenkron)
    }
}
