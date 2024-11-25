using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WEB_253504_Novikov.Domain.Models;
using WEB_253504_Novikov.UI.Services.VehicleService;
using WEB_253504_Novikov.UI.Services.VehicleTypeService;
using WEB_253504_Novikov.UI.Controllers;
using WEB_253504_Novikov.Domain.Entities;

namespace WEB_253504_Novikov.Tests
{
    public class VehicleCatalogControllerTests
    {
        private readonly IVehicleService _vehicleService = Substitute.For<IVehicleService>();
        private readonly IVehicleTypeService _vehicleTypeService = Substitute.For<IVehicleTypeService>();

        private VehicleCatalogController CreateController()
        {
            return new VehicleCatalogController(_vehicleService, _vehicleTypeService);
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenVehicleTypesNotLoaded()
        {
            var controller = CreateController();
            _vehicleTypeService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<VehicleType>>.Error("Vehicle types not loaded.")));

            var result = await controller.Index(null);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Vehicle types not loaded.", notFoundResult.Value);
        }
        [Fact]
        public async Task Index_ReturnsNotFound_WhenVehiclesNotLoaded()
        {
            var controller = CreateController();
            var category = "TestCategory";
            _vehicleTypeService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<VehicleType>>.Success(new List<VehicleType> { new VehicleType { Name = "TestCategory", NormalizedName = "TESTCATEGORY" } })));
            _vehicleService.GetProductListAsync(category).Returns(Task.FromResult(ResponseData<ListModel<Vehicle>>.Error("No such vehicle type")));

            var result = await controller.Index(category);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No such vehicle type", notFoundResult.Value);
        }
        [Fact]
        public async Task Index_PopulatesViewDataWithCategories_WhenCategoriesAreSuccessfullyLoaded()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            httpContext.Request.Headers["X-Requested-With"] = "";

            var expectedCategories = new List<VehicleType> {
                new VehicleType { Name = "Sedan", NormalizedName = "sedan" },
                new VehicleType { Name = "Suv", NormalizedName = "suv" }
            };
            _vehicleTypeService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<VehicleType>>.Success(expectedCategories)));
            _vehicleService.GetProductListAsync().Returns(Task.FromResult(ResponseData<ListModel<Vehicle>>.Success(new ListModel<Vehicle>())));

            var result = await controller.Index(null);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData["categories"]);
            var categoriesInViewData = viewResult.ViewData["categories"] as List<VehicleType>;
            Assert.Equal(expectedCategories, categoriesInViewData);
        }
        [Fact]
        public async Task Index_SetsCurrentCategoryToAll_WhenCategoryIsNull()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            _vehicleTypeService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<VehicleType>>.Success(new List<VehicleType>())));
            _vehicleService.GetProductListAsync(null).Returns(Task.FromResult(ResponseData<ListModel<Vehicle>>.Success(new ListModel<Vehicle>())));

            var result = await controller.Index(null);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Все", viewResult.ViewData["currentVehicleType"]);
        }
        [Fact]
        public async Task Index_SetsCurrentCategoryCorrectly_WhenCategoryIsSpecified()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            string category = "hatch-back";
            var categories = new List<VehicleType>
            {
                new VehicleType { Name = "Sedan", NormalizedName = "sedan" },
                new VehicleType { Name = "Suv", NormalizedName = "suv" },
                new VehicleType { Name = "Pickup", NormalizedName = "pickup" },
                new VehicleType { Name = "Coupe", NormalizedName = "coupe" },
                new VehicleType { Name = "Hatch back", NormalizedName = "hatch-back" },
                new VehicleType { Name = "Cabrio", NormalizedName = "cabrio" }
            };
            _vehicleTypeService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<VehicleType>>.Success(categories)));
            _vehicleService.GetProductListAsync(category).Returns(Task.FromResult(ResponseData<ListModel<Vehicle>>.Success(new ListModel<Vehicle>())));

            var result = await controller.Index(category);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Hatch back", viewResult.ViewData["currentVehicleType"]);
        }
        [Fact]
        public async Task Index_ReturnsViewWithVehiclesCatalogModel_WhenDataIsSuccessfullyLoaded()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            string category = "hatch-back";
            var categories = new List<VehicleType>
            {
                new VehicleType { Name = "Sedan", NormalizedName = "sedan" },
                new VehicleType { Name = "Suv", NormalizedName = "suv" },
                new VehicleType { Name = "Pickup", NormalizedName = "pickup" },
                new VehicleType { Name = "Coupe", NormalizedName = "coupe" },
                new VehicleType { Name = "Hatch back", NormalizedName = "hatch-back" },
                new VehicleType { Name = "Cabrio", NormalizedName = "cabrio" }
            };
            var expectedVehicles = new ListModel<Vehicle>
            {
                Items = new List<Vehicle> { new Vehicle(), new Vehicle(), new Vehicle() },
                CurrentPage = 1,
                TotalPages = 2
            };

            _vehicleTypeService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<VehicleType>>.Success(categories)));
            _vehicleService.GetProductListAsync(category).Returns(Task.FromResult(ResponseData<ListModel<Vehicle>>.Success(expectedVehicles)));

            var result = await controller.Index(category);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CatalogModel>(viewResult.Model);
            Assert.Equal(expectedVehicles, model.ProductsResponse.Data);
        }
    }
}