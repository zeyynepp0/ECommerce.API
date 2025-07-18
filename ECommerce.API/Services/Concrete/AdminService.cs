// Admin servisinin iş mantığı ve gerekli kütüphaneler
using ECommerce.API.Entities.Concrete; // Admin, kullanıcı, sipariş gibi varlık sınıfları
using ECommerce.API.Repository.Abstract; // Repository arayüzleri
using ECommerce.API.Services.Abstract; // Admin servis arayüzü
using ECommerce.API.DTO; // DTO sınıfları

namespace ECommerce.API.Services.Concrete
{
    // Admin işlemleriyle ilgili iş mantığını yöneten servis sınıfı
    public class AdminService : IAdminService
    {
        private readonly IOrderRepository _orderRepository; // Sipariş repository'si
        private readonly IUserRepository _userRepository; // Kullanıcı repository'si
        private readonly IReviewRepository _reviewRepository; // Yorum repository'si
        private readonly IProductRepository _productRepository; // Ürün repository'si
        private readonly IAdminRepository _adminRepository; // Admin repository'si
        private readonly ICategoryRepository _categoryRepository; // Kategori repository'si

        // AdminService constructor: Repository bağımlılıklarını enjekte eder
        public AdminService(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IReviewRepository reviewRepository,
            IProductRepository productRepository,
            IAdminRepository adminRepository,
            ICategoryRepository categoryRepository)
        {
            _orderRepository = orderRepository; // Sipariş repository'sini ata
            _userRepository = userRepository; // Kullanıcı repository'sini ata
            _reviewRepository = reviewRepository; // Yorum repository'sini ata
            _productRepository = productRepository; // Ürün repository'sini ata
            _adminRepository = adminRepository; // Admin repository'sini ata
            _categoryRepository = categoryRepository; // Kategori repository'sini ata
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
                try { totalOrders = await _orderRepository.GetAllAsync(); Console.WriteLine($"Siparişler çekildi: {totalOrders?.Count}"); } catch (Exception ex) { Console.WriteLine($"Siparişler çekilemedi: {ex.Message}"); }
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
                try { totalUsers = await _userRepository.CountAsync(); Console.WriteLine($"Kullanıcılar: {totalUsers}"); } catch (Exception ex) { Console.WriteLine($"Kullanıcılar hatası: {ex.Message}"); }
                try { monthlyNewUsers = await _userRepository.CountByDateRangeAsync(startOfMonth, now); Console.WriteLine($"Aylık yeni kullanıcılar: {monthlyNewUsers}"); } catch (Exception ex) { Console.WriteLine($"Aylık yeni kullanıcılar hatası: {ex.Message}"); }
                try { totalProducts = await _productRepository.CountAsync(); Console.WriteLine($"Ürünler: {totalProducts}"); } catch (Exception ex) { Console.WriteLine($"Ürünler hatası: {ex.Message}"); }
                try { totalCategories = await _categoryRepository.CountAsync(); Console.WriteLine($"Kategoriler: {totalCategories}"); } catch (Exception ex) { Console.WriteLine($"Kategoriler hatası: {ex.Message}"); }
                try { totalReviews = (await _reviewRepository.GetAllAsync()).Count; Console.WriteLine($"Yorumlar: {totalReviews}"); } catch (Exception ex) { Console.WriteLine($"Yorumlar hatası: {ex.Message}"); }
                try { lowStockProducts = await _productRepository.GetLowStockProductsAsync(10); Console.WriteLine($"Düşük stoklu ürünler: {lowStockProducts?.Count}"); } catch (Exception ex) { Console.WriteLine($"Düşük stoklu ürünler hatası: {ex.Message}"); }

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
            var users = await _userRepository.GetAllAsync(); // Tüm kullanıcıları getir
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

        // Admin girişini doğrular
        public async Task<Admin> AuthenticateAsync(string email, string password)
        {
            return await _adminRepository.GetByEmailAndPasswordAsync(email, password); // E-posta ve şifre ile admin getir
        }

        // Admin bilgilerini günceller
        public async Task<bool> UpdateAdminAsync(UpdateAdminDto dto)
        {
            var admin = await _adminRepository.GetByIdAsync(dto.Id); // Admini getir
            if (admin == null) return false; // Admin yoksa false
            if (admin.User == null)
            {
                // User'ı yükle
                admin.User = await _userRepository.GetByIdAsync(admin.UserId); // Kullanıcıyı getir
                if (admin.User == null) return false; // Kullanıcı yoksa false
            }
            admin.User.FirstName = dto.Name; // Adı güncelle (Name yerine FirstName/LastName ayırmak istersen DTO'yu güncelle)
            admin.User.Email = dto.Email; // E-posta güncelle
            if (!string.IsNullOrEmpty(dto.Password))
                admin.User.PasswordHash = dto.Password; // Şifre güncelle
            await _userRepository.SaveAsync(); // Değişiklikleri kaydet
            return true; // Başarılı ise true
        }

        // Belirli bir döneme göre gelir raporu döndürür
        public async Task<RevenueReportDto> GetRevenueReportAsync(string period)
        {
            var orders = await _orderRepository.GetAllAsync();
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

            return new RevenueReportDto
            {
                TotalRevenue = filteredOrders.Sum(o => o.TotalAmount),
                OrderCount = filteredOrders.Count(),
                StartDate = period == "day" ? now.Date : (period == "month" ? new DateTime(now.Year, now.Month, 1) : (period == "year" ? new DateTime(now.Year, 1, 1) : (DateTime?)null)),
                EndDate = now
            };
        }

        // Belirli bir ay ve yıl için yeni kullanıcı sayısını döndürür (henüz uygulanmadı)
        public Task<int> GetNewUsersCountAsync(int month, int year)
        {
            throw new NotImplementedException(); // Henüz uygulanmadı
        }

        // Kullanıcı aktivitelerini döndürür (henüz uygulanmadı)
        public async Task<object> GetUserActivityAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var orders = await _orderRepository.GetAllAsync();
            var reviews = await _reviewRepository.GetAllAsync();

            var activityList = users.Select(u => new DTO.UserActivityDto
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

        // Tüm yorumları döndürür (artık uygulanıyor)
        public async Task<List<object>> GetAllReviewsAsync()
        {
            // User ve Product navigation property'lerini dahil ederek çek
            var reviews = await _reviewRepository.GetAllAsync();
            var userIds = reviews.Select(r => r.UserId).Distinct().ToList();
            var productIds = reviews.Select(r => r.ProductId).Distinct().ToList();

            // User ve Product'ları topluca çek
            var users = await _userRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            var reviewDtos = reviews.Select(r => new DTO.ReviewDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                Comment = r.Comment,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt,
                ProductName = products.FirstOrDefault(p => p.Id == r.ProductId)?.Name ?? string.Empty,
                UserFullName = users.FirstOrDefault(u => u.Id == r.UserId)?.FullName ?? string.Empty
            }).ToList<object>();
            return reviewDtos;
        }

        // Yorumu günceller (henüz uygulanmadı)
        public Task<bool> UpdateReviewAsync(int id, UpdateReviewDto dto)
        {
            throw new NotImplementedException(); // Henüz uygulanmadı
        }

        // Yorumu siler (henüz uygulanmadı)
        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return false;

            _reviewRepository.Delete(review);
            await _reviewRepository.SaveAsync();
            return true;
        }
    }
}
