using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Entities.Concrete
{
    
    /// Kullanıcıların favori ürünlerini temsil eden entity.
   
    public class Favorite
    {
        
        /// Favori kaydının benzersiz kimliği.
       
        public int Id { get; set; }

        
        /// Favori kaydının ait olduğu kullanıcının kimliği (FK).
       
        public int UserId { get; set; }

        
        /// Favori olarak eklenen ürünün kimliği (FK).
       
        public int ProductId { get; set; }

        
        /// Favori olarak eklenen ürün nesnesi (navigation property).
       
        public Product? Product { get; set; }
    }
} 