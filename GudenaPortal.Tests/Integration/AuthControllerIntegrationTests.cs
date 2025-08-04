using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gudena.Api.DTOs;
using Gudena.Api.Repositories;
using Gudena.Api.Services;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GudenaPortal.Tests.Integration
{
    public class AuthAndBasketIntegrationTests
    {
        private readonly AppDbContext _dbContext;
        private readonly AuthService _authService;
        private readonly BusinessProductService _productService;
        private readonly BasketService _basketService;

        public AuthAndBasketIntegrationTests()
        {
            _dbContext = TestDbHelper.GetInMemoryDbContext();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:Key", "ThisIsASecretKeyForTesting123!" },
                    { "Jwt:Issuer", "TestIssuer" },
                    { "Jwt:Audience", "TestAudience" },
                    { "Jwt:DurationInMinutes", "60" }
                })
                .Build();

            var authRepository = new AuthRepository(null!, _dbContext);
            _authService = new AuthService(authRepository, configuration, null!);

            var productRepository = new ProductRepository(_dbContext);
            var basketRepository = new BasketRepository(_dbContext, productRepository);
            _productService = new BusinessProductService(_dbContext);
            _basketService = new BasketService(basketRepository);
        }

        [Fact]
        public async Task Register_Login_AddProductToBasket_ThenRetrieveBasket()
        {
            // Create user
            var user = new ApplicationUser
            {
                UserName = "buyer@example.com",
                Email = "buyer@example.com",
                UserType = "Buyer"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Create an empty basket
            var basket = new Basket
            {
                ApplicationUserId = user.Id,
                BasketItems = new List<BasketItem>()
            };
            _dbContext.Baskets.Add(basket);
            await _dbContext.SaveChangesAsync();

            // Create category
            var category = new Category
            {
                Name = "Electronics",
                Description = "Electronics category",
                CategoryType = "General"
            };
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            // Create product
            var productDto = new ProductCreateDto
            {
                Name = "Test Phone",
                Description = "A test phone",
                Thumbnail = "thumb.jpg",
                Price = 500,
                Stock = 5,
                Unit = "pcs",
                IsDeleted = false,
                ReferenceUrl = "http://phone.com",
                IsReturnable = true,
                CategoryId = category.Id,
                MediaUrls = new List<string>()
            };
            var createResult = await _productService.CreateProductAsync(user.Id, productDto);
            createResult.Should().Be("Product created successfully");

            var createdProduct = await _dbContext.Products.FirstAsync();
            createdProduct.Name.Should().Be("Test Phone");

            // Add product to basket
            var updatedBasket = await _basketService.AddProductToBasketAsync(user.Id, createdProduct.Id, 2);
            updatedBasket.BasketItems.Should().NotBeNullOrEmpty();
            updatedBasket.BasketItems.First().ProductId.Should().Be(createdProduct.Id);
            updatedBasket.BasketItems.First().Amount.Should().Be(2);

            // Retrieve basket and verify
            var retrievedBasket = await _basketService.RetrieveBasketAsync(user.Id, -1);
            retrievedBasket.Should().NotBeNull();
            retrievedBasket.BasketItems.Should().ContainSingle();
            retrievedBasket.BasketItems.First().ProductId.Should().Be(createdProduct.Id);
            retrievedBasket.BasketItems.First().Amount.Should().Be(2);
        }
    }
}