using Gudena.Api.DTOs;
using Gudena.Api.Services.Interfaces;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Gudena.Api.Services
{
    public class BusinessProductService : IBusinessProductService
    {
        private readonly AppDbContext _context;

        public BusinessProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateProductAsync(string userId, ProductCreateDto dto)
        {
            var category = await _context.Categories.FindAsync(dto.CategoryId);
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

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            if (dto.MediaUrls != null && dto.MediaUrls.Any())
            {
                var mediaList = dto.MediaUrls.Select(url => new Media
                {
                    ProductId = product.Id,
                    MediaUrl = url
                }).ToList();

                _context.Media.AddRange(mediaList);
                await _context.SaveChangesAsync();
            }

            return "Product created successfully";
        }

        public async Task<string> UpdateProductAsync(string userId, int productId, ProductCreateDto dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId && p.OwnerId == userId);
            if (product == null) return "Product not found or not owned by you";

            var category = await _context.Categories.FindAsync(dto.CategoryId);
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
            
            await _context.SaveChangesAsync();
            return "Product updated successfully";
        }

        public async Task<string> DeleteProductAsync(string userId, int productId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId && p.OwnerId == userId);
            if (product == null) return "Product not found or not owned by you";

            product.IsDeleted = true;
            await _context.SaveChangesAsync();
            return "Product marked as deleted successfully";
        }

        public async Task<object> GetMyProductsAsync(string userId)
        {
            return await _context.Products
                .Where(p => p.OwnerId == userId && !p.IsDeleted)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Thumbnail,
                    p.Price,
                    p.Stock,
                    p.Unit,
                    p.ReferenceUrl,
                    p.isReturnable,
                    Category = p.Category != null ? p.Category.Name : null,
                    Media = p.Media.Select(m => m.MediaUrl).ToList()
                })
                .ToListAsync();
        }
    }
}
