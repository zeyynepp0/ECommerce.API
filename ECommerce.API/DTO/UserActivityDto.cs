namespace ECommerce.API.DTO
{
    public class UserActivityDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? LastOrderDate { get; set; }
        public DateTime? LastReviewDate { get; set; }
        public int TotalOrders { get; set; }
        public int TotalReviews { get; set; }
    }
} 