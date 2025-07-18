// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
namespace ECommerce.API.DTO
{
    // Sepet ürünü verilerini taşımak için kullanılan DTO sınıfı
    public class CartItemDto
    {
        public int UserId { get; set; } // Kullanıcı ID'si
        public int ProductId { get; set; } // Ürün ID'si
        public int Quantity { get; set; } // Ürün adedi
    }
} 