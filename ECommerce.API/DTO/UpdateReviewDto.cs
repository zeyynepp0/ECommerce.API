// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Yorum güncelleme işlemleri için kullanılan DTO sınıfı
    public class UpdateReviewDto
    {
        public int Id { get; set; } // Yorumun ID'si
        public string Content { get; set; } // Yorum içeriği
        public int Rating { get; set; } // Yorum puanı
        public string LastModifiedBy { get; set; } // Güncelleyen kişi (admin/user)
    }
} 