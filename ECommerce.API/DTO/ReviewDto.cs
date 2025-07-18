namespace ECommerce.API.DTO
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string? ProductName { get; set; }
        public string? UserFullName { get; set; }
    }
} 