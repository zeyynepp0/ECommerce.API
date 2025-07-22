// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Ürün ekleme işlemleri için kullanılan DTO sınıfı
    public class ProductDto
    {
        public int Id { get; set; } // Ürün kimliği
        public string Name { get; set; } // Ürün adı
        public string Description { get; set; } // Ürün açıklaması
        public decimal Price { get; set; } // Ürün fiyatı
        public int Stock { get; set; } // Ürün stok miktarı
        public int CategoryId { get; set; } // Ürünün ait olduğu kategori ID'si
        public string ImageUrl { get; set; } // Ürün görselinin URL'si
        public double Rating { get; set; } // Ortalama puan
        public int ReviewCount { get; set; } // Yorum sayısı
        public string CategoryName { get; set; } // Kategori adı
        public bool IsActive { get; set; }
    }
} 