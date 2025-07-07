
using ECommerce.API.Entities.Abstract;

namespace ECommerce.API.Entities.Concrete
{
    public class Admin :IEntity
    {
         public int AdminId { get; set; }
         public int AdminPassword { get; set; }
        public string AdminName { get; set; }
    }
}
