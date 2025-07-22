using System;

namespace ECommerce.API.Entities.Concrete
{
    /// <summary>
    /// Kargo firmalarını temsil eden entity.
    /// </summary>
    public class ShippingCompany
    {
        /// <summary>
        /// Kargo firmasının benzersiz kimliği.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Kargo firması adı.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Kargo ücreti.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Ücretsiz kargo limiti. Bu tutarın üzerindeki siparişlerde kargo ücreti alınmaz.
        /// </summary>
        public decimal FreeShippingLimit { get; set; }

        /// <summary>
        /// Firma aktif mi?
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Oluşturulma tarihi.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Son güncellenme tarihi.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 