using Xunit;
using FluentAssertions;
using Gudena.Api.Services;
using Gudena.Api.Repositories;
using Gudena.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GudenaPortal.Tests
{
    public class ShippingServiceTests
    {
        [Fact]
        public async Task GetShippingByIdAsync_ReturnsShipping_WhenExists()
        {
            using var context = TestDbHelper.GetInMemoryDbContext();
            var repository = new ShippingRepository(context);
            var service = new ShippingService(repository);

            var shipping = new Shipping
            {
                City = "TestCity",
                Street = "Main Street",
                PostalCode = "12345",
                Country = "TestCountry",
                DeliveryOption = "Standard",
                ShippingNumbers = "SN123",
                ApplicationUserId = "user123",
                BusinessUserId = "business123",
                ShippingStatus = "Pending"
            };
            context.Shippings.Add(shipping);
            await context.SaveChangesAsync();

            var result = await service.GetShippingByIdAsync(shipping.Id);

            result.Should().NotBeNull();
            result.City.Should().Be("TestCity");
        }

        [Fact]
        public async Task GetShippingByIdAsync_ReturnsNull_WhenNotExists()
        {
            using var context = TestDbHelper.GetInMemoryDbContext();
            var repository = new ShippingRepository(context);
            var service = new ShippingService(repository);

            var result = await service.GetShippingByIdAsync(999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetShippingsByUserIdAsync_ReturnsCorrectShippings()
        {
            using var context = TestDbHelper.GetInMemoryDbContext();
            var repository = new ShippingRepository(context);
            var service = new ShippingService(repository);

            // Arrange: Create an order with ApplicationUserId = "user123"
            var order = new Order
            {
                ApplicationUserId = "user123",
                Status = "Pending",
                PaymentMethod = "Card"
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = 1, // dummy product id, assuming FK not enforced in memory
                Quantity = 1,
                PricePerUnit = 100
            };
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            var shipping = new Shipping
            {
                City = "City1",
                Street = "Street1",
                PostalCode = "10000",
                Country = "Country1",
                DeliveryOption = "Standard",
                ShippingNumbers = "SN001",
                ApplicationUserId = "user123",
                BusinessUserId = "business123",
                ShippingStatus = "Completed",
                OrderItems = new List<OrderItem> { orderItem }
            };
            context.Shippings.Add(shipping);

            //shipping with different user
            var otherShipping = new Shipping
            {
                City = "City2",
                Street = "Street2",
                PostalCode = "10001",
                Country = "Country2",
                DeliveryOption = "Express",
                ShippingNumbers = "SN002",
                ApplicationUserId = "otherUser",
                BusinessUserId = "business456",
                ShippingStatus = "Completed"
            };
            context.Shippings.Add(otherShipping);

            await context.SaveChangesAsync();

            // Act
            var result = await service.GetShippingsByUserIdAsync("user123");

            // Assert
            result.Should().HaveCount(1);
            result[0].City.Should().Be("City1");
        }

        [Fact]
        public async Task CreateShippingAsync_SavesAndReturnsShipping()
        {
            using var context = TestDbHelper.GetInMemoryDbContext();
            var repository = new ShippingRepository(context);
            var service = new ShippingService(repository);

            var shipping = new Shipping
            {
                City = "NewCity",
                Street = "Main Street",
                PostalCode = "98765",
                Country = "NewCountry",
                DeliveryOption = "Express",
                ShippingNumbers = "SN789",
                ApplicationUserId = "user456",
                BusinessUserId = "business789",
                ShippingStatus = "Pending"
            };

            var result = await service.CreateShippingAsync(shipping, new List<int>());

            result.Id.Should().BeGreaterThan(0);
            var dbShipping = await context.Shippings.FindAsync(result.Id);
            dbShipping.Should().NotBeNull();
            dbShipping.City.Should().Be("NewCity");
        }

        [Fact]
        public async Task CleanUpPreviews_RemovesPreviewShippings()
        {
            using var context = TestDbHelper.GetInMemoryDbContext();
            var repository = new ShippingRepository(context);
            var service = new ShippingService(repository);

            context.Shippings.AddRange(
                new Shipping
                {
                    City = "City1",
                    Street = "Street1",
                    PostalCode = "10000",
                    Country = "Country1",
                    DeliveryOption = "Standard",
                    ShippingNumbers = "SN001",
                    ApplicationUserId = "user123",
                    BusinessUserId = "business123",
                    ShippingStatus = "Preview"
                },
                new Shipping
                {
                    City = "City2",
                    Street = "Street2",
                    PostalCode = "10001",
                    Country = "Country2",
                    DeliveryOption = "Express",
                    ShippingNumbers = "SN002",
                    ApplicationUserId = "user123",
                    BusinessUserId = "business123",
                    ShippingStatus = "Completed"
                }
            );
            await context.SaveChangesAsync();

            await service.CleanUpPreviews("user123");

            var previews = await repository.RetrievePreviews("user123");
            previews.Should().BeEmpty();
        }
    }
}
