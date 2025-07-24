using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Entities.Concrete
{
    public class CampaignCategory
    {
        [Key]
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
} 