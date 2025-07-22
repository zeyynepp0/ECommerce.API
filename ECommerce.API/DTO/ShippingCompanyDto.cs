namespace ECommerce.API.DTO
{
    /// <summary>
    /// Kargo firması için DTO.
    /// </summary>
    public class ShippingCompanyDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Ücretsiz kargo limiti. Bu tutarın üzerindeki siparişlerde kargo ücreti alınmaz.
        /// </summary>
        public decimal FreeShippingLimit { get; set; }
    }
} 