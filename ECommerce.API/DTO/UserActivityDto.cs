namespace ECommerce.API.DTO
{
    public class UserActivityDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public DateTime? LastReviewDate { get; set; }
        public int TotalOrders { get; set; }
        public int TotalReviews { get; set; }
    }
} 