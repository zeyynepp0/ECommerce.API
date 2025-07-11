namespace ECommerce.API.Entities.Concrete
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Order Order { get; set; } = null!; // FK to Order (Cascade)
        public Product Product { get; set; } = null!; // FK to Product (Restrict)
    }
}
