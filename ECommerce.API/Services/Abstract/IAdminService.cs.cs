// Admin servis arayüzü ve gerekli kütüphaneler
using ECommerce.API.DTO; // DTO sınıfları
using ECommerce.API.Entities.Concrete; // Admin ve diğer varlık sınıfları
using System.Collections.Generic; // Liste işlemleri için
using System.Threading.Tasks; // Asenkron işlemler için

namespace ECommerce.API.Services.Abstract
{
    // Admin işlemleri için servis arayüzü
    public interface IAdminService
    {
        Task<AdminDashboardDto> GetDashboardDataAsync(); // Admin paneli için özet verileri getirir (asenkron)
        Task<List<UserDto>> GetAllUsersAsync(); // Tüm kullanıcıları getirir (asenkron)
        Task<RevenueReportDto> GetRevenueReportAsync(string period); // Belirli bir döneme göre gelir raporu getirir (asenkron)
        Task<int> GetNewUsersCountAsync(int month, int year); // Belirli bir ay ve yıl için yeni kullanıcı sayısını getirir (asenkron)
        Task<object> GetUserActivityAsync(); // Kullanıcı aktivitelerini getirir (asenkron)
        Task<List<object>> GetAllReviewsAsync(); // Tüm yorumları getirir (asenkron)
        Task<bool> UpdateReviewAsync(int id, UpdateReviewDto dto); // Yorumu günceller (asenkron)
        Task<bool> DeleteReviewAsync(int id); // Yorumu siler (asenkron)
        Task<bool> UpdateAdminAsync(UpdateAdminDto dto); // Admin bilgilerini günceller (asenkron)
    }
}
