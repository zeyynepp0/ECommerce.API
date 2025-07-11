using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.DTO
{
    public class AddressUserDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string AddressTitle { get; set; } = string.Empty;
        [Required]
        public string Street { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string State { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactSurname { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
    }
}
