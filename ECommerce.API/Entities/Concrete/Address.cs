using ECommerce.API.Entities.Concrete; // User entity için

namespace ECommerce.API.Entities.Concrete
{
    /// Kullanıcıya ait adres bilgisini tutan entity.
    public class Address
    {
        /// Adresin benzersiz kimliği.
        public int Id { get; set; }
        /// Adresin ait olduğu kullanıcının kimliği (FK).
        public int UserId { get; set; }
        /// Adres başlığı (ör: Ev, İş, vb.).
        public string AddressTitle { get; set; } = string.Empty;
        /// Sokak bilgisi.
        public string Street { get; set; } = string.Empty;
        /// Şehir bilgisi.
        public string City { get; set; } = string.Empty;
        /// İl/eyalet bilgisi.
        public string State { get; set; } = string.Empty;
        /// Posta kodu bilgisi.
        public string PostalCode { get; set; } = string.Empty;
        /// Ülke bilgisi.
        public string Country { get; set; } = string.Empty;
        /// Adrese ait kişinin adı.
        public string ContactName { get; set; } = string.Empty;
        /// Adrese ait kişinin soyadı.
        public string ContactSurname { get; set; } = string.Empty;
        /// Adrese ait kişinin telefon numarası.
        public string ContactPhone { get; set; } = string.Empty;  
        /// Adresin ait olduğu kullanıcı nesnesi (navigation property).
        public User User { get; set; } // FK to User
    }
}
