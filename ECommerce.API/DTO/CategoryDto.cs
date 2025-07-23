// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Kategori verilerini taşımak için kullanılan DTO sınıfı
    public class CategoryDto
    {
        public int Id { get; set; } // Kategorinin benzersiz kimliği
        public string Name { get; set; } // Kategori adı
        public string ImageUrl { get; set; } // Kategoriye ait görselin URL'si
        public bool IsActive { get; set; } // Kategori aktif mi?
    }
} 