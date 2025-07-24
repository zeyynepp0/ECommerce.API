using System.Collections.Generic;

namespace ECommerce.API.DTO
{
    public class EligibleCampaignRequestDto
    {
        public List<int> ProductIds { get; set; } = new List<int>();
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
} 