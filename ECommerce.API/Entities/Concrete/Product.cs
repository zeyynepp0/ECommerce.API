using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Entities.Concrete
{
    
    /// Ürünleri temsil eden entity.
   
    public class Product
    {
        
        /// Ürünün benzersiz kimliği.
       
        public int Id { get; set; }

        
        /// Ürün adı.
       
        public string Name { get; set; } = string.Empty;

        
        /// Ürün açıklaması.
       
        public string Description { get; set; } = string.Empty;

        
        /// Ürün fiyatı.
       
        public decimal Price { get; set; }

        
        /// Ürün stok adedi.
       
        public int Stock { get; set; }

        
        /// Ürünün ait olduğu kategori kimliği (FK).
       
        public int CategoryId { get; set; }

        
        /// Ürünün ait olduğu kategori nesnesi (navigation property).
       
        public Category Category { get; set; } = null!; // 1 Category : N Product (Restrict)

        
        /// Ürüne ait yorumlar (1 Product : N Review).
       
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        
        /// Ürünün favori olarak eklendiği kayıtlar (1 Product : N Favorite).
       
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        
        /// Ürün resmi yolu (zorunlu).
       
        public string ImageUrl { get; set; } = string.Empty;
    }
}
