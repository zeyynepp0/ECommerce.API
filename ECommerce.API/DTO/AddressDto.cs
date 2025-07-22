namespace ECommerce.API.DTO
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string AddressTitle { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactSurname { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
} 