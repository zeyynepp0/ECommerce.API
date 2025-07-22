using System.Net;

namespace ECommerce.API.Entities.Concrete
{
    
    /// Sistemdeki kullanıcıları temsil eden entity.
   
    public class User
    {
        
        /// Kullanıcının benzersiz kimliği.
       
        public int Id { get; set; }

        
        /// Kullanıcının adı.
       
        public string FirstName { get; set; } = string.Empty;

        
        /// Kullanıcının soyadı.
       
        public string LastName { get; set; } = string.Empty;

        
        /// Kullanıcının tam adı (Ad + Soyad).
       
        public string FullName { get { return FirstName + " " + LastName; } }

        
        /// Kullanıcının doğum tarihi.
       
        public DateTime BirthDate { get; set; }

        
        /// Kullanıcının e-posta adresi.
       
        public string Email { get; set; } = string.Empty;

        
        /// Kullanıcının telefon numarası.
       
        public string Phone { get; set; } = string.Empty;

        
        /// E-posta adresinin onaylanıp onaylanmadığı.
       
        public bool EmailConfirmed { get; set; }

        
        /// Kullanıcının şifre hash değeri.
       
        public string PasswordHash { get; set; } = string.Empty;

        
        /// Kullanıcının rolü (User, Admin, Employee).
       
        public UserRole Role { get; set; }

        
        /// Kullanıcının oluşturulma tarihi.
       
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        /// Kullanıcının sahip olduğu adresler (1 User : N Address).
       
        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        
        /// Kullanıcının verdiği siparişler (1 User : N Order).
       
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        
        /// Kullanıcının yaptığı ürün yorumları (1 User : N Review).
       
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        
        /// Kullanıcının favori ürünleri (1 User : N Favorite).
       
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }

    
    /// Kullanıcı rollerini tanımlayan enum.
   
    public enum UserRole
    {
        
        /// Standart kullanıcı.
       
        User = 0,
        
        /// Yönetici kullanıcı.
       
        Admin = 1,
        
        /// Çalışan kullanıcı.
       
        Employee = 2
    }
}
