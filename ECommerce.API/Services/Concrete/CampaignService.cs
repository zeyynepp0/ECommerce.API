using System.Collections.Generic;
using System.Linq;
using ECommerce.API.DTO;
using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _repository;
        public CampaignService(ICampaignRepository repository)
        {
            _repository = repository;
        }

        public CampaignDto GetById(int id)
        {
            var campaign = _repository.GetById(id);
            return ToDto(campaign);
        }

        public List<CampaignDto> GetAll()
        {
            return _repository.GetAll().Select(ToDto).ToList();
        }

        public void Add(CampaignDto dto)
        {
            var entity = ToEntity(dto);
            _repository.Add(entity);
            if (dto.ProductIds.Any())
                _repository.AddProducts(entity.Id, dto.ProductIds);
            if (dto.CategoryIds.Any())
                _repository.AddCategories(entity.Id, dto.CategoryIds);
        }

        public void Update(CampaignDto dto)
        {
            var entity = ToEntity(dto);
            _repository.Update(entity);
            _repository.RemoveProducts(entity.Id, _repository.GetProductsForCampaign(entity.Id).Select(p => p.Id).ToList());
            _repository.RemoveCategories(entity.Id, _repository.GetCategoriesForCampaign(entity.Id).Select(c => c.Id).ToList());
            if (dto.ProductIds.Any())
                _repository.AddProducts(entity.Id, dto.ProductIds);
            if (dto.CategoryIds.Any())
                _repository.AddCategories(entity.Id, dto.CategoryIds);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public void AddProducts(int campaignId, List<int> productIds)
        {
            _repository.AddProducts(campaignId, productIds);
        }

        public void AddCategories(int campaignId, List<int> categoryIds)
        {
            _repository.AddCategories(campaignId, categoryIds);
        }

        public void RemoveProducts(int campaignId, List<int> productIds)
        {
            _repository.RemoveProducts(campaignId, productIds);
        }

        public void RemoveCategories(int campaignId, List<int> categoryIds)
        {
            _repository.RemoveCategories(campaignId, categoryIds);
        }

        public void ToggleActive(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return;
            entity.IsActive = !entity.IsActive;
            _repository.Update(entity);
        }

        public List<int> GetProductIdsForCampaign(int campaignId)
        {
            return _repository.GetProductsForCampaign(campaignId).Select(p => p.Id).ToList();
        }

        public List<int> GetCategoryIdsForCampaign(int campaignId)
        {
            return _repository.GetCategoriesForCampaign(campaignId).Select(c => c.Id).ToList();
        }

        // Mapping helpers
        private CampaignDto ToDto(Campaign c)
        {
            if (c == null) return null;
            return new CampaignDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Type = (int)c.Type,
                Percentage = c.Percentage,
                Amount = c.Amount,
                BuyQuantity = c.BuyQuantity,
                PayQuantity = c.PayQuantity,
                MinOrderAmount = c.MinOrderAmount,
                IsActive = c.IsActive,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ProductIds = c.CampaignProducts?.Select(cp => cp.ProductId).ToList() ?? new List<int>(),
                CategoryIds = c.CampaignCategories?.Select(cc => cc.CategoryId).ToList() ?? new List<int>()
            };
        }

        private Campaign ToEntity(CampaignDto dto)
        {
            return new Campaign
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Type = (CampaignType)dto.Type,
                Percentage = dto.Percentage,
                Amount = dto.Amount,
                BuyQuantity = dto.BuyQuantity,
                PayQuantity = dto.PayQuantity,
                MinOrderAmount = dto.MinOrderAmount,
                IsActive = dto.IsActive,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
        }
    }
} 