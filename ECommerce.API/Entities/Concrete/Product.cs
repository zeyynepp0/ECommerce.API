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
    }
}
