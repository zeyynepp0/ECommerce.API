

using ECommerce.API.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Entities.Concrete
{
    
    /// Sistemdeki yönetici kullanıcıları temsil eden entity.
   
    public class Admin
    {
        
        /// Yöneticinin benzersiz kimliği.
       
        [Key]
        public int Id { get; set; }

        
        /// Yöneticinin e-posta adresi.
       
        public string Email { get; set; } = string.Empty;

        
        /// Yöneticinin şifre hash değeri.
       
        public string PasswordHash { get; set; } = string.Empty;

        
        /// Yöneticinin bağlı olduğu kullanıcı kimliği (FK).
       
        public int UserId { get; set; } // User tablosu ile ilişki

        
        /// Yöneticinin bağlı olduğu kullanıcı nesnesi (navigation property).
       
        public User User { get; set; } = null!;
    }
}
