// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Gelir raporu verilerini taşımak için kullanılan DTO sınıfı
    public class RevenueReportDto
    {
        public decimal TotalRevenue { get; set; } // Toplam gelir
        public int OrderCount { get; set; } // Sipariş sayısı
        public string Period { get; set; } // Rapor dönemi (ör: daily, monthly, yearly)
        public DateTime? StartDate { get; set; } // Rapor başlangıç tarihi
        public DateTime EndDate { get; set; } // Rapor bitiş tarihi
    }
} 