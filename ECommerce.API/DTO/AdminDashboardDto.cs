// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    public class RevenueGraphDto
    {
        public List<string> Labels { get; set; }
        public List<decimal> Values { get; set; }
    }

    // Admin paneli için özet verileri taşıyan DTO sınıfı
    public class AdminDashboardDto
    {
        public decimal? DailyRevenue { get; set; } // Günlük gelir
        public decimal? MonthlyRevenue { get; set; } // Aylık gelir
        public decimal? YearlyRevenue { get; set; } // Yıllık gelir
        public int? NewUsersThisMonth { get; set; } // Bu ay eklenen yeni kullanıcı sayısı
        public int? TotalUsers { get; set; } // Toplam kullanıcı sayısı
        public int? TotalOrders { get; set; } // Toplam sipariş sayısı
        public int? MonthlyOrders { get; set; } // Bu ayki sipariş sayısı
        public int? MonthlyNewUsers { get; set; } // Bu ay eklenen yeni kullanıcı sayısı (tekrar)
        public int? TotalProducts { get; set; } // Toplam ürün sayısı
        public int? LowStockCount { get; set; } // Stokta azalan ürün sayısı
        public int? TotalCategories { get; set; } // Toplam kategori sayısı
        public int? TotalReviews { get; set; } // Toplam yorum sayısı
        public decimal? TotalRevenue { get; set; } // Toplam gelir
        public List<object> RecentOrders { get; set; } = new List<object>(); // Son siparişler listesi
        public RevenueGraphDto RevenueGraph { get; set; }
    }
} 