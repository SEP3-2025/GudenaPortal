using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gudena.Api.DTOs;
using Gudena.Api.Repositories;
using Gudena.Api.Services;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GudenaPortal.Tests
{
    public class AuthServiceTests
    {
        private readonly IConfiguration _config;

        public AuthServiceTests()
        {
            // Fake config for JWT generation
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "test_secret_key_1234567890" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" },
                { "Jwt:DurationInMinutes", "60" }
            };
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public async Task RegisterBuyerAsync_ShouldCreateUserAndAccountDetails()
        {
            // Arrange
            using var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = BuildUserManager(context);
            var repository = new AuthRepository(userManager, context);
            var service = new AuthService(repository, _config, userManager);

            var dto = new BuyerRegisterDto
            {
                Email = "buyer@test.com",
                Password = "Pass123!",
                FirstName = "John",
                LastName = "Doe",
                City = "City",
                Street = "Street",
                PostalCode = "12345",
                Country = "Country",
                PhoneNumber = "123456789"
            };

            // Act
            var result = await service.RegisterBuyerAsync(dto);

            // Assert
            result.Should().Be("Registration successful.");
            context.Users.Count().Should().Be(1);
            context.AccountDetails.Count().Should().Be(1);
        }

        [Fact]
        public async Task RegisterBusinessAsync_ShouldCreateUserAndAccountDetails()
        {
            // Arrange
            using var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = BuildUserManager(context);
            var repository = new AuthRepository(userManager, context);
            var service = new AuthService(repository, _config, userManager);

            var dto = new BusinessRegisterDto
            {
                Email = "business@test.com",
                Password = "Pass123!",
                FirstName = "Jane",
                LastName = "Doe",
                CompanyName = "MyCompany",
                City = "City",
                Street = "Street",
                PostalCode = "12345",
                Country = "Country",
                PhoneNumber = "123456789"
            };

            // Act
            var result = await service.RegisterBusinessAsync(dto);

            // Assert
            result.Should().Be("Business registration successful.");
            context.Users.Count().Should().Be(1);
            context.AccountDetails.Single().CompanyName.Should().Be("MyCompany");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            using var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = BuildUserManager(context);
            var repository = new AuthRepository(userManager, context);
            var service = new AuthService(repository, _config, userManager);

            var user = new ApplicationUser
            {
                Email = "login@test.com",
                UserName = "login@test.com",
                UserType = "Buyer"
            };
            await userManager.CreateAsync(user, "Pass123!");

            var dto = new LoginDto { Email = "login@test.com", Password = "Pass123!" };

            // Act
            var token = await service.LoginAsync(dto);

            // Assert
            token.Should().NotBeNullOrEmpty();
            token.Should().Contain(".");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenInvalidCredentials()
        {
            // Arrange
            using var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = BuildUserManager(context);
            var repository = new AuthRepository(userManager, context);
            var service = new AuthService(repository, _config, userManager);

            var dto = new LoginDto { Email = "wrong@test.com", Password = "WrongPass" };

            // Act
            var token = await service.LoginAsync(dto);

            // Assert
            token.Should().BeNull();
        }

        // Helper to build a working UserManager with in-memory stores
        private static UserManager<ApplicationUser> BuildUserManager(AppDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            return new UserManager<ApplicationUser>(
                store,
                null,
                new PasswordHasher<ApplicationUser>(),
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                null,
                null,
                null,
                null
            );
        }
    }
}
