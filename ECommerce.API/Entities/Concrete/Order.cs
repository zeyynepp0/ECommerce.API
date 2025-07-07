namespace ECommerce.API.Entities.Concrete
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } // 1 Order : N OrderItem (Cascade)
        public User User { get; set; }                          // FK to User (Cascade)
       
    }
}
