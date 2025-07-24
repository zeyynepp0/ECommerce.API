using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Entities.Concrete
{
    public class CampaignProduct
    {
        [Key]
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
} 