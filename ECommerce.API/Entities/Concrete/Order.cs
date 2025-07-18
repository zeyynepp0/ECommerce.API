namespace ECommerce.API.Entities.Concrete
{
    
    /// Siparişleri temsil eden entity.
   
    public class Order
    {
        
        /// Siparişin benzersiz kimliği.
       
        public int Id { get; set; }

        
        /// Siparişi veren kullanıcının kimliği (FK).
       
        public int UserId { get; set; }

        
        /// Siparişin teslim edileceği adresin kimliği (FK).
       
        public int AddressId { get; set; }

        
        /// Sipariş tarihi.
       
        public DateTime OrderDate { get; set; }

        
        /// Siparişin toplam tutarı.
       
        public decimal TotalAmount { get; set; }

        
        /// Siparişin durumu (örn: Pending, Approved, Rejected).
       
        public string Status { get; set; } = string.Empty;

        
        /// Kargo şirketi bilgisi.
       
        public string ShippingCompany { get; set; } = string.Empty;

        
        /// Ödeme yöntemi bilgisi.
       
        public string PaymentMethod { get; set; } = string.Empty;

        
        /// Siparişteki ürünler (1 Order : N OrderItem).
       
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        
        /// Siparişi veren kullanıcı nesnesi (navigation property).
       
        public User User { get; set; } = null!; // FK to User (Cascade)

        
        /// Siparişin teslim edileceği adres nesnesi (navigation property).
       
        public Address Address { get; set; } = null!; // FK to Address (Cascade)
    }
}
