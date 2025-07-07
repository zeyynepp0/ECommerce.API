namespace ECommerce.API.Entities.Concrete
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public Product Product { get; set; } // FK to Product(Cascade)
        public User User { get; set; }       // FK to User (Restrict)
    }
}
