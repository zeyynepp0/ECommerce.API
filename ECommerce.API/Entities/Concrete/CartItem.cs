namespace ECommerce.API.Entities.Concrete
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product? Product { get; set; } // FK to Product (Restrict)
        public User? User { get; set; }       // FK to User (Cascade)
    }
}
