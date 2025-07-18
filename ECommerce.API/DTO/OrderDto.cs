// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.DTO
{
    // Sipariş verilerini taşımak için kullanılan DTO sınıfı
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public string ShippingCompany { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public AddressDto Address { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    // Sipariş kalemi verilerini taşımak için kullanılan DTO sınıfı
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
    }

    // Ödeme verilerini taşımak için kullanılan DTO sınıfı
    public class PaymentDto
    {
        [Required]
        public string CardNumber { get; set; } = string.Empty; // Kart numarası
        
        [Required]
        public string CardHolderName { get; set; } = string.Empty; // Kart sahibi adı
        
        [Required]
        public string ExpiryMonth { get; set; } = string.Empty; // Son kullanma ayı
        
        [Required]
        public string ExpiryYear { get; set; } = string.Empty; // Son kullanma yılı
        
        [Required]
        public string Cvv { get; set; } = string.Empty; // Kart güvenlik kodu
        
        public int OrderId { get; set; } // İlgili siparişin ID'si
    }
} 