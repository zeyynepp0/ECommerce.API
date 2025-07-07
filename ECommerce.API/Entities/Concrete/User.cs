using System.Net;

namespace ECommerce.API.Entities.Concrete
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Address> Addresses { get; set; } // 1 User : N Address (Cascade)
        public ICollection<Order> Orders { get; set; }      // 1 User : N Order (Cascade)
        public ICollection<Review> Reviews { get; set; }     // 1 User : N Review (Restrict)
    }
}
