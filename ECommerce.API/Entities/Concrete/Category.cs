namespace ECommerce.API.Entities.Concrete
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } // 1 Category : N Product

        public Category()
        {
            Products = new List<Product>();
        }
    }
}
