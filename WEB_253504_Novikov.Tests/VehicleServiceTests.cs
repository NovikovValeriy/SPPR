using Microsoft.EntityFrameworkCore;
using WEB_253504_Novikov.API.Data;
using WEB_253504_Novikov.API.Services.VehicleServices;
using WEB_253504_Novikov.Domain.Entities;

namespace WEB_253504_Novikov.Tests
{
    public class VehicleServiceTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            return context;
        }

        [Fact]
        public async Task GetVehicleListAsync_ReturnsFirstPageWithThreeItemsAndCalculatesTotalPagesCorrectly()
        {
            var context = CreateInMemoryDbContext();

            var categories = new List<VehicleType>
            {
                new VehicleType { Name = "Sedan", NormalizedName = "sedan", Id = 1 },
                new VehicleType { Name = "Suv", NormalizedName = "suv", Id = 2 },
                new VehicleType { Name = "Pickup", NormalizedName = "pickup", Id = 3 },
                new VehicleType { Name = "Coupe", NormalizedName = "coupe", Id = 4 },
                new VehicleType { Name = "Hatch back", NormalizedName = "hatch-back", Id = 5 },
                new VehicleType { Name = "Cabrio", NormalizedName = "cabrio", Id = 6 }
            };

            await context.VehicleTypes.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            context.Vehicles.AddRange(
                new Vehicle { Name = "Vehicle 1", Description = "Description 1", Cost = 2000, TypeId = categories[5].Id },
                new Vehicle { Name = "Vehicle 2", Description = "Description 2", Cost = 3000, TypeId = categories[4].Id },
                new Vehicle { Name = "Vehicle 3", Description = "Description 3", Cost = 4000, TypeId = categories[3].Id },
                new Vehicle { Name = "Vehicle 4", Description = "Description 4", Cost = 5000, TypeId = categories[2].Id },
                new Vehicle { Name = "Vehicle 5", Description = "Description 5", Cost = 6000, TypeId = categories[1].Id },
                new Vehicle { Name = "Vehicle 6", Description = "Description 6", Cost = 7000, TypeId = categories[0].Id }
            );

            await context.SaveChangesAsync();

            var service = new VehicleService(context, null);

            var result = await service.GetProductListAsync(null);

            Assert.True(result.Successfull);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Items.Count);

            int totalPages = (int)Math.Ceiling(6 / (double)3);
            Assert.Equal(totalPages, result.Data.TotalPages);
        }

        [Fact]
        public async Task GetVehicleListAsync_ReturnsCorrectPage_WhenSpecificPageIsRequested()
        {
            var context = CreateInMemoryDbContext();

            var categories = new List<VehicleType>
            {
                new VehicleType { Name = "Suv", NormalizedName = "suv" }
            };

            await context.VehicleTypes.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            context.Vehicles.AddRange(
                new Vehicle { Name = "Vehicle 1", Description = "Description 1", Cost = 2000, TypeId = categories[0].Id },
                new Vehicle { Name = "Vehicle 2", Description = "Description 2", Cost = 3000, TypeId = categories[0].Id },
                new Vehicle { Name = "Vehicle 3", Description = "Description 3", Cost = 4000, TypeId = categories[0].Id },
                new Vehicle { Name = "Vehicle 4", Description = "Description 4", Cost = 5000, TypeId = categories[0].Id },
                new Vehicle { Name = "Vehicle 5", Description = "Description 5", Cost = 6000, TypeId = categories[0].Id },
                new Vehicle { Name = "Vehicle 6", Description = "Description 6", Cost = 7000, TypeId = categories[0].Id }
            );

            await context.SaveChangesAsync();

            var service = new VehicleService(context, null);

            int requestedPageNo = 2;
            int pageSize = 3;

            var result = await service.GetProductListAsync(null, requestedPageNo, pageSize);

            Assert.True(result.Successfull);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(requestedPageNo, result.Data.CurrentPage);

            Assert.Contains(result.Data.Items, m => m.Name == "Vehicle 4");
            Assert.Contains(result.Data.Items, m => m.Name == "Vehicle 5");
            Assert.Contains(result.Data.Items, m => m.Name == "Vehicle 6");
        }

        [Fact]
        public async Task GetvehicleListAsync_FiltersVehiclesByCategoryCorrectly()
        {
            var context = CreateInMemoryDbContext();

            var categories = new List<VehicleType>
            {
                new VehicleType { Name = "Sedan", NormalizedName = "sedan" },
                new VehicleType { Name = "Suv", NormalizedName = "suv" }
            };

            await context.VehicleTypes.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            context.Vehicles.AddRange(
                new Vehicle { Name = "Vehicle 1", Description = "Description 1", Cost = 2000, TypeId = categories[0].Id },
                new Vehicle { Name = "Vehicle 2", Description = "Description 2", Cost = 3000, TypeId = categories[1].Id },
                new Vehicle { Name = "Vehicle 3", Description = "Description 3", Cost = 4000, TypeId = categories[0].Id },
                new Vehicle { Name = "Vehicle 4", Description = "Description 4", Cost = 5000, TypeId = categories[1].Id },
                new Vehicle { Name = "Vehicle 5", Description = "Description 5", Cost = 6000, TypeId = categories[1].Id },
                new Vehicle { Name = "Vehicle 6", Description = "Description 6", Cost = 7000, TypeId = categories[0].Id }
            );

            await context.SaveChangesAsync();

            var service = new VehicleService(context, null);

            string categoryNormalizedName = "suv";

            var result = await service.GetProductListAsync(categoryNormalizedName);

            Assert.True(result.Successfull);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Items.Count);

            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(1, result.Data.TotalPages);


            foreach (var vehicle in result.Data.Items)
            {
                Assert.Equal(2, vehicle.TypeId);
            }
        }

        [Fact]
        public async Task GetVehicleListAsync_DoesNotAllowPageSizeGreaterThanMaxPageSize()
        {
            var context = CreateInMemoryDbContext();

            var categories = new List<VehicleType>
            {
                new VehicleType { Name = "Sedan", NormalizedName = "sedan", Id = 1 }
            };

            await context.VehicleTypes.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            for (int i = 1; i <= 25; i++)
            {
                context.Vehicles.Add(new Vehicle { Name = $"Vehicle {i}", Description = $"Description {i}", Cost = 2000 + i * 100, TypeId = categories[0].Id });
            }

            await context.SaveChangesAsync();

            var service = new VehicleService(context, null);

            int requestedPageSize = 50;
            int maxPageSize = 20;

            var result = await service.GetProductListAsync(null, 1, requestedPageSize);

            Assert.True(result.Successfull);
            Assert.NotNull(result.Data);
            Assert.Equal(maxPageSize, result.Data.Items.Count);
            Assert.Equal(1, result.Data.CurrentPage);
        }
        [Fact]
        public async Task GetVehicleListAsync_ReturnsError_WhenPageNumberExceedsTotalPages()
        {
            var context = CreateInMemoryDbContext();

            var categories = new List<VehicleType>
            {
                new VehicleType { Name = "Sedan", NormalizedName = "sedan", Id = 1 }
            };

            await context.VehicleTypes.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            for (int i = 1; i <= 5; i++)
            {
                context.Vehicles.Add(new Vehicle { Name = $"Vehicle {i}", Description = $"Description {i}", Cost = 2000 + i * 100, TypeId = categories[0].Id });
            }

            await context.SaveChangesAsync();

            var service = new VehicleService(context, null);

            int requestedPageNo = 3;
            int pageSize = 3;

            var result = await service.GetProductListAsync(null, requestedPageNo, pageSize);

            Assert.False(result.Successfull);
            Assert.Equal("No such page", result.ErrorMessage);
        }
    }
}
