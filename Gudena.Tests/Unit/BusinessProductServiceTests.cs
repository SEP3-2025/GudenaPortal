using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gudena.Api.DTOs;
using Gudena.Api.Repositories;
using Gudena.Api.Services;
using Gudena.Data.Entities;
using Xunit;

namespace GudenaPortal.Tests
{
    public class BusinessProductServiceTests
    {
        [Fact]
        public async Task CreateProductAsync_ShouldCreateProduct_WhenCategoryExists()
        {
            // Arrange
            using var context = TestDbHelper.GetInMemoryDbContext();

            context.Categories.Add(new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic devices",
                CategoryType = "Main"
            });
            await context.SaveChangesAsync();

            // use real repository with in-memory db
            var repository = new BusinessProductRepository(context);
            var service = new BusinessProductService(repository);

            var dto = new ProductCreateDto
            {
                Name = "Test Product",
                Description = "Description",
                Thumbnail = "thumb.jpg",
                Price = 100,
                Stock = 10,
                Unit = "pcs",
                IsDeleted = false,
                ReferenceUrl = "http://example.com",
                IsReturnable = true,
                CategoryId = 1,
                MediaUrls = new List<string> { "img1.jpg" }
            };

            // Act
            var result = await service.CreateProductAsync("user1", dto);

            // Assert
            result.Should().Be("Product created successfully");
            context.Products.Count().Should().Be(1);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdate_WhenProductOwnedByUser()
        {
            // Arrange
            using var context = TestDbHelper.GetInMemoryDbContext();

            context.Categories.Add(new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic devices",
                CategoryType = "Main"
            });
            var product = new Product
            {
                Id = 1,
                Name = "Old Name",
                Description = "Old Description",
                Thumbnail = "old.jpg",
                Price = 50,
                Stock = 5,
                Unit = "pcs",
                ReferenceUrl = "http://old.com",
                isReturnable = false,
                CategoryId = 1,
                OwnerId = "user1"
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repository = new BusinessProductRepository(context);
            var service = new BusinessProductService(repository);

            var dto = new ProductCreateDto
            {
                Name = "New Name",
                Description = "Updated Description",
                Thumbnail = "new.jpg",
                Price = 75,
                Stock = 20,
                Unit = "pcs",
                IsDeleted = false,
                ReferenceUrl = "http://new.com",
                IsReturnable = true,
                CategoryId = 1,
                MediaUrls = new List<string> { "img_updated.jpg" }
            };

            // Act
            var result = await service.UpdateProductAsync("user1", 1, dto);

            // Assert
            result.Should().Be("Product updated successfully");
            var updated = await context.Products.FindAsync(1);
            updated.Name.Should().Be("New Name");
            updated.Price.Should().Be(75);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldSoftDelete_WhenOwnedByUser()
        {
            // Arrange
            using var context = TestDbHelper.GetInMemoryDbContext();

            context.Categories.Add(new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic devices",
                CategoryType = "Main"
            });
            var product = new Product
            {
                Id = 1,
                Name = "Product to delete",
                Description = "desc",
                Thumbnail = "thumb.jpg",
                Price = 100,
                Stock = 10,
                Unit = "pcs",
                ReferenceUrl = "http://url.com",
                isReturnable = true,
                CategoryId = 1,
                OwnerId = "user1"
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var repository = new BusinessProductRepository(context);
            var service = new BusinessProductService(repository);

            // Act
            var result = await service.DeleteProductAsync("user1", 1);

            // Assert
            result.Should().Be("Product marked as deleted successfully");
            (await context.Products.FindAsync(1))!.IsDeleted.Should().BeTrue();
        }
    }
}
