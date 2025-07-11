namespace ECommerce.API.Entities.Concrete
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty; // Sipariş durumu (örn: Pending, Approved, Rejected)
        public string ShippingCompany { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // 1 Order : N OrderItem (Cascade)
        public User User { get; set; } = null!;                          // FK to User (Cascade)
        public Address Address { get; set; } = null!;                    // FK to Address (Cascade)
    }
}
