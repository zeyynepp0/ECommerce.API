// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Gelir verilerini taşımak için kullanılan DTO sınıfı
    public class RevenueDto
    {
        public decimal TotalRevenue { get; set; } // Toplam gelir
        public int OrderCount { get; set; } // Sipariş sayısı
        public string Period { get; set; } // Rapor dönemi (ör: aylık, yıllık)
    }
} 