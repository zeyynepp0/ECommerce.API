using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Entities.Concrete
{
    public enum OrderStatus
    {
        Pending,        // Onay Bekliyor
        Approved,       // Onaylandı
        Preparing,      // Hazırlanıyor
        Shipped,        // Kargoya Verildi
        Delivered,      // Teslim Edildi
        Cancelled,      // İptal Edildi
        Returned,       // İade Talebi
        Refunded        // İade Edildi
    }

    public enum AdminOrderStatus
    {
        None = 0,
        InReview = 1,
        Approved = 2,
        Rejected = 3,
        Completed = 4
    }

    public enum PaymentMethodType
    {
        CreditCard,
        DebitCard,
        BankTransfer,
        Cash
    }

    public enum UserOrderRequest
    {
        None = 0,
        Cancel = 1,
        Return = 2
    }

    public static class OrderStatusExtensions
    {
        public static string ToDisplayString(this OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Onay Bekliyor",
                OrderStatus.Approved => "Onaylandı",
                OrderStatus.Preparing => "Hazırlanıyor",
                OrderStatus.Shipped => "Kargoya Verildi",
                OrderStatus.Delivered => "Teslim Edildi",
                OrderStatus.Cancelled => "İptal Edildi",
                OrderStatus.Returned => "İade Talebi",
                OrderStatus.Refunded => "İade Edildi",
                _ => status.ToString()
            };
        }
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
        public string DeliveryPersonName { get; set; }
        public string DeliveryPersonPhone { get; set; }
        public int? ShippingCompanyId { get; set; }
        public ShippingCompany ShippingCompany { get; set; }
        public AdminOrderStatus AdminStatus { get; set; } = AdminOrderStatus.None;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public UserOrderRequest UserRequest { get; set; } = UserOrderRequest.None;
    }
}
