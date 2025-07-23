// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Ürün güncelleme işlemleri için kullanılan DTO sınıfı
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty; // Ürün adı
        public string Description { get; set; } = string.Empty; // Ürün açıklaması
        public decimal Price { get; set; } // Ürün fiyatı
        public int Stock { get; set; } // Ürün stok miktarı
        public int CategoryId { get; set; } // Ürünün ait olduğu kategori ID'si
        public string ImageUrl { get; set; } = string.Empty; // Ürün görselinin URL'si
        public bool IsActive { get; set; } // Ürün aktif mi?
    }
} 