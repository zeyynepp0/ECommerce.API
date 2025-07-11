

using ECommerce.API.Entities.Abstract;

namespace ECommerce.API.Entities.Concrete
{
    public class Admin :IEntity
    {
         public int AdminId { get; set; }
         public int UserId { get; set; } // User tablosu ile ilişki
         public User User { get; set; }

        public Admin()
        {
            User = new User();
        }
    }
}
