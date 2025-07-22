// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.DTO
{
    // Sipariş verilerini taşımak için kullanılan DTO sınıfı
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
        public string DeliveryPersonName { get; set; }
        public string DeliveryPersonPhone { get; set; }
        public int? ShippingCompanyId { get; set; }
        public string? ShippingCompanyName { get; set; }
        public AdminOrderStatus AdminStatus { get; set; } = AdminOrderStatus.None;
        public int AddressId { get; set; }
        public AddressDto Address { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public string StatusText => Status.ToDisplayString();
        public UserOrderRequest UserRequest { get; set; } = UserOrderRequest.None;
        public string UserRequestText => UserRequest == UserOrderRequest.Cancel ? "İptal Talebi" : UserRequest == UserOrderRequest.Return ? "İade Talebi" : "Yok";
    }

    // Sipariş kalemi verilerini taşımak için kullanılan DTO sınıfı
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