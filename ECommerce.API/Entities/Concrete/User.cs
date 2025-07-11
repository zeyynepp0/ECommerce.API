using System.Net;

namespace ECommerce.API.Entities.Concrete
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get { return FirstName + " " + LastName; } }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } // Kullanıcı rolü: User, Admin, Employee
        public ICollection<Address> Addresses { get; set; } // 1 User : N Address (Cascade)
        public ICollection<Order> Orders { get; set; }      // 1 User : N Order (Cascade)
        public ICollection<Review> Reviews { get; set; }     // 1 User : N Review (Restrict)
        public ICollection<Favorite> Favorites { get; set; } // 1 User : N Favorite (Cascade)

        public User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            PasswordHash = string.Empty;
            Addresses = new List<Address>();
            Orders = new List<Order>();
            Reviews = new List<Review>();
            Favorites = new List<Favorite>();
        }
    }

    public enum UserRole
    {
        User =0,
        Admin=1,
        Employee=2
    }
}
