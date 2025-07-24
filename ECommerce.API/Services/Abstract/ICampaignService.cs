using System.Collections.Generic;
using ECommerce.API.DTO;

namespace ECommerce.API.Services.Abstract
{
    public interface ICampaignService
    {
        CampaignDto GetById(int id);
        List<CampaignDto> GetAll();
        void Add(CampaignDto campaignDto);
        void Update(CampaignDto campaignDto);
        void Delete(int id);
        void AddProducts(int campaignId, List<int> productIds);
        void AddCategories(int campaignId, List<int> categoryIds);
        void RemoveProducts(int campaignId, List<int> productIds);
        void RemoveCategories(int campaignId, List<int> categoryIds);
        List<int> GetProductIdsForCampaign(int campaignId);
        List<int> GetCategoryIdsForCampaign(int campaignId);
        void ToggleActive(int id);
    }
} 