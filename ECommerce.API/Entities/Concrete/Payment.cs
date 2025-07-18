namespace ECommerce.API.Entities.Concrete
{
    
    /// Sipariş ödemelerini temsil eden entity.
   
    public class Payment
    {
        
        /// Ödeme kaydının benzersiz kimliği.
       
        public int Id { get; set; }

        
        /// Ödeme yapılan siparişin kimliği (FK).
       
        public int OrderId { get; set; }

        
        /// Ödeme yöntemi bilgisi.
       
        public string PaymentMethod { get; set; } = string.Empty;

        
        /// Ödeme durumu bilgisi.
       
        public string PaymentStatus { get; set; } = string.Empty;

        
        /// Ödeme tarihi.
       
        public DateTime PaymentDate { get; set; }

        
        /// Ödeme yapılan sipariş nesnesi (navigation property).
       
        public Order Order { get; set; } = null!; // 1 Order : 1 Payment (Cascade)
    }
}
