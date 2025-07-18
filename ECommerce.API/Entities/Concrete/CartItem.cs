namespace ECommerce.API.Entities.Concrete
{
    
    /// Kullanıcıların sepetlerindeki ürünleri temsil eden entity.
   
    public class CartItem
    {
        
        /// Sepet kaydının benzersiz kimliği.
       
        public int Id { get; set; }

        
        /// Sepet kaydının ait olduğu kullanıcının kimliği (FK).
       
        public int UserId { get; set; }

        
        /// Sepetteki ürünün kimliği (FK).
       
        public int ProductId { get; set; }

        
        /// Sepetteki ürün adedi.
       
        public int Quantity { get; set; }

        
        /// Sepetteki ürün nesnesi (navigation property).
       
        public Product? Product { get; set; } // FK to Product (Restrict)

        
        /// Sepet kaydının ait olduğu kullanıcı nesnesi (navigation property).
       
        public User? User { get; set; } // FK to User (Cascade)
    }
}
