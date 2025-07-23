namespace ECommerce.API.Entities.Concrete
{
    
    /// Ürün kategorilerini temsil eden entity.
   
    public class Category
    {
        
        /// Kategorinin benzersiz kimliği.
       
        public int Id { get; set; }

        
        /// Kategori adı.
       
        public string Name { get; set; } = string.Empty;

        
        /// Kategoriye ait görselin yolu.
       
        public string ImageUrl { get; set; } = string.Empty;

        
        /// Kategoriye ait ürünler (1 Category : N Product).
       
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public bool IsActive { get; set; } = true;
    }
}
