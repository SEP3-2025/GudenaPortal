using Gudena.Api.DTOs;
using Gudena.Data;
using Gudena.Data.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/business/products")]
    [Authorize(Policy = "BusinessOnly")]
    public class BusinessProductsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public BusinessProductsController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetMyProducts()
        {
            var userId = User.FindFirstValue("uid");
            if (userId == null)
                return Unauthorized();

            var products = await _context.Products
                .Where(p => p.OwnerId == userId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Thumbnail,
                    p.Price,
                    p.Stock,
                    p.Unit,
                    p.IsDeleted,
                    p.ReferenceUrl,
                    p.isReturnable,
                    Category = p.Category != null ? p.Category.Name : null
                })
                .ToListAsync();

            return Ok(products);
        }

        /// <summary>
        /// Create a new product owned by the logged-in business
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto dto)
        {
            var userId = User.FindFirstValue("uid");
            if (userId == null)
                return Unauthorized();

            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
                return BadRequest("Invalid category ID");

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

            return Ok("Product created successfully");
        }

        /// <summary>
        /// Update product owned by the logged-in business
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductCreateDto dto)
        {
            var userId = User.FindFirstValue("uid");
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);
            if (product == null)
                return NotFound("Product not found or not owned by you");

            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
                return BadRequest("Invalid category ID");

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

            await _context.SaveChangesAsync();
            return Ok("Product updated successfully");
        }

        /// <summary>
        /// Soft delete product owned by the logged-in business
        /// (keeps it in DB but marks as deleted)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userId = User.FindFirstValue("uid");
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);

            if (product == null)
                return NotFound("Product not found or not owned by you");

            // Soft delete instead of removing from DB
            product.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok("Product marked as deleted successfully");
        }
        
        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreProduct(int id)
        {
            var userId = User.FindFirstValue("uid");
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);

            if (product == null)
                return NotFound("Product not found or not owned by you");

            product.IsDeleted = false;
            await _context.SaveChangesAsync();

            return Ok("Product restored successfully");
        }
    }
}
