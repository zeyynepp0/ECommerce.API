namespace ECommerce.API.Entities.Concrete
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public Order Order { get; set; } // 1 Order : 1 Payment (Cascade)
    }
}
