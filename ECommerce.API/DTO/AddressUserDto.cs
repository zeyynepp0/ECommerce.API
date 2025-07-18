// DTO (Data Transfer Object) sınıflarının bulunduğu namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.DTO
{
    // Kullanıcı adres verilerini taşımak için kullanılan DTO sınıfı
    public class AddressUserDto
    {
        public int Id { get; set; } // Adres ID'si
        public int UserId { get; set; } // Kullanıcı ID'si
        [Required]
        public string AddressTitle { get; set; } = string.Empty; // Adres başlığı
        [Required]
        public string Street { get; set; } = string.Empty; // Sokak bilgisi
        [Required]
        public string City { get; set; } = string.Empty; // Şehir
        [Required]
        public string State { get; set; } = string.Empty; // Eyalet
        [Required]
        public string PostalCode { get; set; } = string.Empty; // Posta kodu
        [Required]
        public string Country { get; set; } = string.Empty; // Ülke
        public string ContactName { get; set; } = string.Empty; // İletişim adı
        public string ContactSurname { get; set; } = string.Empty; // İletişim soyadı
        public string ContactPhone { get; set; } = string.Empty; // İletişim telefonu
    }
}
