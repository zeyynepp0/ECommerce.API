// Admin servisinin iş mantığı ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Admin, kullanıcı, sipariş gibi varlık sınıfları
using ECommerce.API.Repository.Abstract; // Repository arayüzleri
using ECommerce.API.Services.Abstract; // Admin servis arayüzü
using ECommerce.API.DTO; // DTO sınıfları
using ECommerce.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce.API.Services.Concrete
{
    // Admin işlemleriyle ilgili iş mantığını yöneten servis sınıfı
    public class AdminService : IAdminService
    {
        private readonly MyDbContext _context;

        // AdminService constructor: Repository bağımlılıklarını enjekte eder
        public AdminService(MyDbContext context)
        {
            _context = context;
        }

        // Admin paneli için özet dashboard verilerini döndürür
        public async Task<AdminDashboardDto> GetDashboardDataAsync()
        {
            try
            {
                Console.WriteLine("DashboardData: Başladı");
                var now = DateTime.Now;
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var startOfYear = new DateTime(now.Year, 1, 1);

                List<Order> totalOrders = null;
                try { totalOrders = await _context.Orders.ToListAsync(); Console.WriteLine($"Siparişler çekildi: {totalOrders?.Count}"); } catch (Exception ex) { Console.WriteLine($"Siparişler çekilemedi: {ex.Message}"); }
                IEnumerable<Order> monthlyOrders = null;
                IEnumerable<Order> yearlyOrders = null;
                decimal? dailyRevenue = null;
                decimal? monthlyRevenue = null;
                decimal? yearlyRevenue = null;
                int? totalUsers = null;
                int? monthlyNewUsers = null;
                int? totalProducts = null;
                int? totalCategories = null;
                int? totalReviews = null;
                decimal? totalRevenue = null;
                List<Product> lowStockProducts = null;
                List<object> recentOrders = null;

                if (totalOrders != null)
                {
                    try { monthlyOrders = totalOrders.Where(o => o.OrderDate >= startOfMonth); Console.WriteLine($"Aylık siparişler: {monthlyOrders?.Count()}"); } catch (Exception ex) { Console.WriteLine($"Aylık siparişler hatası: {ex.Message}"); }
                    try { yearlyOrders = totalOrders.Where(o => o.OrderDate >= startOfYear); Console.WriteLine($"Yıllık siparişler: {yearlyOrders?.Count()}"); } catch (Exception ex) { Console.WriteLine($"Yıllık siparişler hatası: {ex.Message}"); }
                    try { dailyRevenue = totalOrders.Where(o => o.OrderDate.Date == now.Date).Sum(o => o.TotalAmount); Console.WriteLine($"Günlük gelir: {dailyRevenue}"); } catch (Exception ex) { Console.WriteLine($"Günlük gelir hatası: {ex.Message}"); }
                    try { monthlyRevenue = monthlyOrders?.Sum(o => o.TotalAmount); Console.WriteLine($"Aylık gelir: {monthlyRevenue}"); } catch (Exception ex) { Console.WriteLine($"Aylık gelir hatası: {ex.Message}"); }
                    try { yearlyRevenue = yearlyOrders?.Sum(o => o.TotalAmount); Console.WriteLine($"Yıllık gelir: {yearlyRevenue}"); } catch (Exception ex) { Console.WriteLine($"Yıllık gelir hatası: {ex.Message}"); }
                    try { totalRevenue = totalOrders.Sum(o => o.TotalAmount); Console.WriteLine($"Toplam gelir: {totalRevenue}"); } catch (Exception ex) { Console.WriteLine($"Toplam gelir hatası: {ex.Message}"); }
                    try { recentOrders = totalOrders.OrderByDescending(o => o.OrderDate).Take(5).Cast<object>().ToList(); Console.WriteLine($"Son siparişler: {recentOrders?.Count}"); } catch (Exception ex) { Console.WriteLine($"Son siparişler hatası: {ex.Message}"); }
                }
                try { totalUsers = await _context.Users.CountAsync(); Console.WriteLine($"Kullanıcılar: {totalUsers}"); } catch (Exception ex) { Console.WriteLine($"Kullanıcılar hatası: {ex.Message}"); }
                try { monthlyNewUsers = await GetNewUsersCountAsync(30); Console.WriteLine($"Aylık yeni kullanıcılar: {monthlyNewUsers}"); } catch (Exception ex) { Console.WriteLine($"Aylık yeni kullanıcılar hatası: {ex.Message}"); }
                try { totalProducts = await _context.Products.CountAsync(); Console.WriteLine($"Ürünler: {totalProducts}"); } catch (Exception ex) { Console.WriteLine($"Ürünler hatası: {ex.Message}"); }
                try { totalCategories = await _context.Categories.CountAsync(); Console.WriteLine($"Kategoriler: {totalCategories}"); } catch (Exception ex) { Console.WriteLine($"Kategoriler hatası: {ex.Message}"); }
                try { totalReviews = await _context.Reviews.CountAsync(); Console.WriteLine($"Yorumlar: {totalReviews}"); } catch (Exception ex) { Console.WriteLine($"Yorumlar hatası: {ex.Message}"); }
                try { lowStockProducts = await GetLowStockProductsAsync(10); Console.WriteLine($"Düşük stoklu ürünler: {lowStockProducts?.Count}"); } catch (Exception ex) { Console.WriteLine($"Düşük stoklu ürünler hatası: {ex.Message}"); }

                // --- Aylık gelir grafiği ---
                var revenueGraph = new RevenueGraphDto
                {
                    Labels = new List<string>(),
                    Values = new List<decimal>()
                };
                if (totalOrders != null && totalOrders.Count > 0)
                {
                    var last12Months = Enumerable.Range(0, 12)
                        .Select(i => now.AddMonths(-i))
                        .OrderBy(d => d)
                        .ToList();
                    foreach (var dt in last12Months)
                    {
                        var monthOrders = totalOrders.Where(o => o.OrderDate.Year == dt.Year && o.OrderDate.Month == dt.Month);
                        revenueGraph.Labels.Add(dt.ToString("yyyy-MM"));
                        revenueGraph.Values.Add(monthOrders.Sum(o => o.TotalAmount));
                    }
                }

                Console.WriteLine("DashboardData: DTO oluşturuluyor");
                var dto = new AdminDashboardDto
                {
                    TotalOrders = totalOrders?.Count(),
                    MonthlyOrders = monthlyOrders?.Count(),
                    TotalUsers = totalUsers,
                    MonthlyNewUsers = monthlyNewUsers,
                    NewUsersThisMonth = monthlyNewUsers,
                    TotalProducts = totalProducts,
                    LowStockCount = lowStockProducts?.Count,
                    DailyRevenue = dailyRevenue,
                    MonthlyRevenue = monthlyRevenue,
                    YearlyRevenue = yearlyRevenue,
                    RecentOrders = recentOrders ?? new List<object>(),
                    TotalCategories = totalCategories,
                    TotalReviews = totalReviews,
                    TotalRevenue = totalRevenue,
                    RevenueGraph = revenueGraph ?? new RevenueGraphDto { Labels = new List<string>(), Values = new List<decimal>() }
                };
                Console.WriteLine("DashboardData: DTO döndürüldü");
                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DASHBOARD HATASI: " + ex.ToString());
                throw;
            }
        }

        // Tüm kullanıcıları DTO olarak döndürür
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync(); // Tüm kullanıcıları getir
            return users.Select(u => new UserDto
            {
                Id = u.Id, // Kullanıcı ID
                FirstName = u.FirstName, // Adı
                LastName = u.LastName, // Soyadı
                Email = u.Email, // E-posta
                Phone = u.Phone, // Telefon
                Role = u.Role.ToString(), // Rol
                EmailConfirmed = u.EmailConfirmed, // E-posta onay durumu
                BirthDate = u.BirthDate, // Doğum tarihi
                OrderCount = u.Orders.Count(), // Sipariş sayısı
                TotalSpent = u.Orders.Sum(o => o.TotalAmount) // Toplam harcama
            }).ToList();
        }

        // Tüm siparişleri OrderDto olarak döndürür
        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Address)
                .Include(o => o.ShippingCompany)
                .Include(o => o.User)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                UserEmail = o.User?.Email ?? string.Empty,
                CreatedAt = o.CreatedAt,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                ShippingCost = o.ShippingCost,
                PaymentMethod = o.PaymentMethod,
                DeliveryPersonName = o.DeliveryPersonName,
                DeliveryPersonPhone = o.DeliveryPersonPhone,
                ShippingCompanyId = o.ShippingCompanyId,
                ShippingCompanyName = o.ShippingCompany?.Name ?? string.Empty,
                AdminStatus = o.AdminStatus,
                AddressId = o.AddressId,
                Address = o.Address != null ? new AddressDto
                {
                    Id = o.Address.Id,
                    AddressTitle = o.Address.AddressTitle,
                    Street = o.Address.Street,
                    City = o.Address.City,
                    State = o.Address.State,
                    PostalCode = o.Address.PostalCode,
                    Country = o.Address.Country,
                    ContactName = o.Address.ContactName,
                    ContactSurname = o.Address.ContactSurname,
                    ContactPhone = o.Address.ContactPhone
                } : null,
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    ProductName = oi.Product != null ? oi.Product.Name : string.Empty,
                    ProductImage = oi.Product != null ? oi.Product.ImageUrl : string.Empty
                }).ToList(),
                UserRequest = o.UserRequest
                // UserRequestText ataması kaldırıldı
            }).ToList();
        }

        // Admin girişini doğrular
        public async Task<Admin> AuthenticateAsync(string email, string password)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Email == email && a.PasswordHash == password);
        }

        // Admin bilgilerini günceller
        public async Task<bool> UpdateAdminAsync(int adminId, UpdateAdminDto dto)
        {
            var admin = await _context.Admins.FindAsync(adminId);
            if (admin == null) return false;
            
            admin.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
                admin.PasswordHash = dto.Password;
            
            await _context.SaveChangesAsync();
            return true;
        }

        // Belirli bir döneme göre gelir raporu döndürür
        public async Task<List<RevenueReportDto>> GetRevenueReportAsync(string period)
        {
            var orders = await _context.Orders.ToListAsync();
            DateTime now = DateTime.Now;
            IEnumerable<Order> filteredOrders = orders;

            switch (period?.ToLower())
            {
                case "day":
                    filteredOrders = orders.Where(o => o.OrderDate.Date == now.Date);
                    break;
                case "month":
                    filteredOrders = orders.Where(o => o.OrderDate.Year == now.Year && o.OrderDate.Month == now.Month);
                    break;
                case "year":
                    filteredOrders = orders.Where(o => o.OrderDate.Year == now.Year);
                    break;
                default:
                    // Tüm siparişler
                    break;
            }

            return new List<RevenueReportDto>
            {
                new RevenueReportDto
                {
                    TotalRevenue = filteredOrders.Sum(o => o.TotalAmount),
                    OrderCount = filteredOrders.Count(),
                    StartDate = period == "day" ? now.Date : (period == "month" ? new DateTime(now.Year, now.Month, 1) : (period == "year" ? new DateTime(now.Year, 1, 1) : (DateTime?)null)),
                    EndDate = now
                }
            };
        }

        // Belirli gün sayısı için yeni kullanıcı sayısını döndürür
        public async Task<int> GetNewUsersCountAsync(int days)
        {
            var startDate = DateTime.Now.AddDays(-days);
            return await _context.Users.CountAsync(u => u.CreatedAt >= startDate);
        }

        // Kullanıcı aktivitelerini döndürür
        public async Task<List<UserActivityDto>> GetUserActivityAsync()
        {
            var users = await _context.Users.ToListAsync();
            var orders = await _context.Orders.ToListAsync();
            var reviews = await _context.Reviews.ToListAsync();

            var activityList = users.Select(u => new UserActivityDto
            {
                UserId = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                LastOrderDate = orders.Where(o => o.UserId == u.Id).OrderByDescending(o => o.OrderDate).FirstOrDefault()?.OrderDate,
                LastReviewDate = reviews.Where(r => r.UserId == u.Id).OrderByDescending(r => r.CreatedAt).FirstOrDefault()?.CreatedAt,
                TotalOrders = orders.Count(o => o.UserId == u.Id),
                TotalReviews = reviews.Count(r => r.UserId == u.Id)
            }).ToList();

            return activityList;
        }

        // Tüm yorumları döndürür
        public async Task<List<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _context.Reviews.ToListAsync();
            var userIds = reviews.Select(r => r.UserId).Distinct().ToList();
            var productIds = reviews.Select(r => r.ProductId).Distinct().ToList();

            var users = await _context.Users.ToListAsync();
            var products = await _context.Products.ToListAsync();

            var reviewDtos = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                Comment = r.Comment,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt,
                ProductName = products.FirstOrDefault(p => p.Id == r.ProductId)?.Name ?? string.Empty,
                UserFullName = users.FirstOrDefault(u => u.Id == r.UserId)?.FullName ?? string.Empty
            }).ToList();
            return reviewDtos;
        }

        // Yorumu günceller
        public async Task<bool> UpdateReviewAsync(int reviewId, UpdateReviewDto reviewDto)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return false;

            review.Comment = reviewDto.Content;
            review.Rating = reviewDto.Rating;
            review.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // Yorumu siler
        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        // Sipariş durumunu günceller
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;
            
            order.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderAdminStatusAsync(int orderId, Entities.Concrete.AdminOrderStatus adminStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new Exception("Sipariş bulunamadı.");

            order.AdminStatus = adminStatus;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        // Düşük stoklu ürünleri getirir
        private async Task<List<Product>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.Products.Where(p => p.StockQuantity <= threshold).ToListAsync();
        }
    }
}
