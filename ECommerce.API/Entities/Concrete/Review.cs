namespace ECommerce.API.Entities.Concrete
{
    
    /// Ürünlere yapılan kullanıcı yorumlarını temsil eden entity.
   
    public class Review
    {
        
        /// Yorumun benzersiz kimliği.
       
        public int Id { get; set; }

        
        /// Yorum yapılan ürünün kimliği (FK).
       
        public int ProductId { get; set; }

        
        /// Yorumu yapan kullanıcının kimliği (FK).
       
        public int UserId { get; set; }

        
        /// Yorum metni.
       
        public string Comment { get; set; } = string.Empty;

        
        /// Yorumun puan değeri (1-5 arası).
       
        public int Rating { get; set; }

        
        /// Yorumun oluşturulma tarihi.
       
        public DateTime CreatedAt { get; set; }

        /// Yorumun güncellenme tarihi.
       
        public DateTime? UpdatedAt { get; set; }

        // Yorumun silinip silinmediği
        public bool IsDeleted { get; set; } = false;
        // Yorumun silinme zamanı (varsa)
        public DateTime? DeletedAt { get; set; }
        // Son değişikliği yapan ("admin" veya "user")
        public string? LastModifiedBy { get; set; }
        // Son değişiklik zamanı
        public DateTime? LastModifiedAt { get; set; }

        
        /// Yorum yapılan ürün nesnesi (navigation property).
       
        public Product Product { get; set; } = null!; // FK to Product(Cascade)

        
        /// Yorumu yapan kullanıcı nesnesi (navigation property).
       
        public User User { get; set; } = null!; // FK to User (Restrict)
    }
}
