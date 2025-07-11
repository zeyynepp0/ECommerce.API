using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.DTO
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public string ShippingCompany { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PaymentDto
    {
        [Required]
        public string CardNumber { get; set; } = string.Empty;
        
        [Required]
        public string CardHolderName { get; set; } = string.Empty;
        
        [Required]
        public string ExpiryMonth { get; set; } = string.Empty;
        
        [Required]
        public string ExpiryYear { get; set; } = string.Empty;
        
        [Required]
        public string Cvv { get; set; } = string.Empty;
        
        public int OrderId { get; set; }
    }
} 