using Gudena.Api.DTOs;
using Gudena.Api.Repositories.Interfaces;
using Gudena.Api.Services.Interfaces;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Gudena.Api.Services
{
        public class BusinessProductService : IBusinessProductService
    {
        private readonly IBusinessProductRepository _repository;

        public BusinessProductService(IBusinessProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> CreateProductAsync(string userId, ProductCreateDto dto)
        {
            var category = await _repository.GetCategoryAsync(dto.CategoryId);
            if (category == null) return "Invalid category ID";

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Thumbnail = dto.Thumbnail,
                Price = dto.Price,
                Stock = dto.Stock,
                Unit = dto.Unit,
                IsDeleted = dto.IsDeleted,
                ReferenceUrl = dto.ReferenceUrl,
                isReturnable = dto.IsReturnable,
                CategoryId = dto.CategoryId,
                OwnerId = userId
            };

            await _repository.AddProductAsync(product);
            await _repository.SaveChangesAsync();

            if (dto.MediaUrls != null && dto.MediaUrls.Any())
            {
                product.Media = dto.MediaUrls.Select(url => new Media
                {
                    ProductId = product.Id,
                    MediaUrl = url
                }).ToList();

                await _repository.SaveChangesAsync();
            }

            return "Product created successfully";
        }

        public async Task<string> UpdateProductAsync(string userId, int productId, ProductCreateDto dto)
        {
            var product = await _repository.GetProductAsync(productId, userId);
            if (product == null) return "Product not found or not owned by you";

            var category = await _repository.GetCategoryAsync(dto.CategoryId);
            if (category == null) return "Invalid category ID";

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Thumbnail = dto.Thumbnail;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.Unit = dto.Unit;
            product.IsDeleted = dto.IsDeleted;
            product.ReferenceUrl = dto.ReferenceUrl;
            product.isReturnable = dto.IsReturnable;
            product.CategoryId = dto.CategoryId;
            product.Media = dto.MediaUrls != null && dto.MediaUrls.Any()
                ? dto.MediaUrls.Select(m => new Media()
                {
                    ProductId = product.Id,
                    MediaUrl = m
                }).ToList()
                : null;

            await _repository.SaveChangesAsync();
            return "Product updated successfully";
        }

        public async Task<string> DeleteProductAsync(string userId, int productId)
        {
            var product = await _repository.GetProductAsync(productId, userId);
            if (product == null) return "Product not found or not owned by you";

            product.IsDeleted = true;
            await _repository.SaveChangesAsync();
            return "Product marked as deleted successfully";
        }

        public async Task<object> GetMyProductsAsync(string userId)
        {
            return await _repository.GetMyProductsAsync(userId);
        }

        public async Task<object?> GetProduct(string? userId, int productId)
        {
            var product = await _repository.GetProductAsync(productId, userId);
            if (product == null)
                return "Product not found or not owned by you";

            return product;
        }
    }
}
