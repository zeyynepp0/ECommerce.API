using ECommerce.API.Entities.Concrete;
using ECommerce.API.DTO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ECommerce.API.Services.Abstract
{
    public interface IAdminService
    {
        Task UpdateOrderAdminStatusAsync(int orderId, Entities.Concrete.AdminOrderStatus adminStatus);
        Task<bool> UpdateAdminAsync(int adminId, UpdateAdminDto adminDto);
        Task<List<RevenueReportDto>> GetRevenueReportAsync(string period);
        Task<int> GetNewUsersCountAsync(int days);
        Task<List<UserActivityDto>> GetUserActivityAsync();
        Task<List<ReviewDto>> GetAllReviewsAsync();
        Task<bool> UpdateReviewAsync(int reviewId, UpdateReviewDto reviewDto);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<AdminDashboardDto> GetDashboardDataAsync();
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<Admin> AuthenticateAsync(string email, string password);
        Task<List<OrderDto>> GetAllOrdersAsync();
    }
} 