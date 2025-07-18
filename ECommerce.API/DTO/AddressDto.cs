namespace ECommerce.API.DTO
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string AddressTitle { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string ContactName { get; set; }
        public string ContactSurname { get; set; }
        public string ContactPhone { get; set; }
    }
} 