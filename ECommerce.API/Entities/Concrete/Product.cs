using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Entities.Concrete
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } // 1 Category : N Product (Restrict)
        public ICollection<Review> Reviews { get; set; }    // 1 Product : N Review (Cascade)
        public ICollection<Favorite> Favorites { get; set; } // 1 Product : N Favorite (Cascade)
        //[Required]
        public string ImageUrl { get; set; } // Ürün resmi yolu (zorunlu)

        public Product()
        {
            Name = string.Empty;
            Description = string.Empty;
            Category = new Category();
            Reviews = new List<Review>();
            Favorites = new List<Favorite>();
            ImageUrl = string.Empty;
        }
    }
}
