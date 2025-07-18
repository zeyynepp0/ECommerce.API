namespace ECommerce.API.Entities.Concrete
{
    
    /// Sipariş içerisindeki ürünleri temsil eden entity.
   
    public class OrderItem
    {
        
        /// Sipariş kaleminin benzersiz kimliği.
       
        public int Id { get; set; }

        
        /// İlgili siparişin kimliği (FK).
       
        public int OrderId { get; set; }

        
        /// Sipariş edilen ürünün kimliği (FK).
       
        public int ProductId { get; set; }

        
        /// Sipariş edilen ürün adedi.
       
        public int Quantity { get; set; }

        
        /// Ürünün sipariş anındaki birim fiyatı.
       
        public decimal UnitPrice { get; set; }

        
        /// İlgili sipariş nesnesi (navigation property).
       
        public Order Order { get; set; } = null!; // FK to Order (Cascade)

        
        /// Sipariş edilen ürün nesnesi (navigation property).
       
        public Product Product { get; set; } = null!; // FK to Product (Restrict)
    }
}
