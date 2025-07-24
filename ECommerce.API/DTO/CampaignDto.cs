using System;
using System.Collections.Generic;

namespace ECommerce.API.DTO
{
    public class CampaignDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; } // CampaignType
        public decimal? Percentage { get; set; }
        public decimal? Amount { get; set; }
        public int? BuyQuantity { get; set; }
        public int? PayQuantity { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>();
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
} 