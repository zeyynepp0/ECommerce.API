

using ECommerce.API.Entities.Abstract;

namespace ECommerce.API.Entities.Concrete
{
    
    /// Sistemdeki yönetici kullanıcıları temsil eden entity.
   
    public class Admin
    {
        
        /// Yöneticinin benzersiz kimliği.
       
        public int AdminId { get; set; }

        
        /// Yöneticinin bağlı olduğu kullanıcı kimliği (FK).
       
        public int UserId { get; set; } // User tablosu ile ilişki

        
        /// Yöneticinin bağlı olduğu kullanıcı nesnesi (navigation property).
       
        public User User { get; set; } = null!;
    }
}
