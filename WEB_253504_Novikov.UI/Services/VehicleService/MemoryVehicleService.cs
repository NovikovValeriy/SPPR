using Microsoft.AspNetCore.Mvc;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.Domain.Models;
using WEB_253504_Novikov.UI.Services.VehicleTypeService;

namespace WEB_253504_Novikov.UI.Services.VehicleService
{
    public class MemoryVehicleService : IVehicleService
    {
        List<Vehicle> _vehicles;
        IVehicleTypeService _vehicleTypeService;
        IConfiguration _configuration;
        public MemoryVehicleService(
            [FromServices] IConfiguration config,
            IVehicleTypeService vehicleTypeService
            )
        {
            _vehicleTypeService = vehicleTypeService;
            _configuration = config;
            SetupData();
        }

        private void SetupData()
        {
            var vehicleTypes = _vehicleTypeService.GetCategoryListAsync()
                .Result
                .Data;

            _vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, Name = "Ford Crown Victoria", Description = "American sedan frequently used by police", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("sedan")), Cost = 5000, ImagePath = "/Images/crown_victoria.jpg"},
                new Vehicle { Id = 2, Name = "Ford F150", Description = "Popular utility truck", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("pickup")), Cost = 25000, ImagePath = "/Images/f150.jpg" },
                new Vehicle { Id = 3, Name = "Cadillac Escalade", Description = "American luxury SUV", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("suv")), Cost = 32000, ImagePath = "/Images/escalade.jpg" },
                new Vehicle { Id = 4, Name = "Mercedes McLaren", Description = "Luxury coupe", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("coupe")), Cost = 120000, ImagePath = "/Images/mclaren.jpg" },
                new Vehicle { Id = 5, Name = "Lada Granta", Description = "Russian inexpensive sedan", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("sedan")), Cost = 3000, ImagePath = "Images/granta.jpg" },
                new Vehicle { Id = 6, Name = "Suzuki Swift", Description = "Small and quick korean hatch-back", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("hatch-back")), Cost = 8000, ImagePath = "Images/swift.jpg" },
                new Vehicle { Id = 7, Name = "Toyota Hilux", Description = "A reliant utility vehicle", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("pickup")), Cost = 12000, ImagePath = "Images/hilux.jpg" },
                new Vehicle { Id = 8, Name = "Kia Rio", Description = "Popular korean sedan", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("sedan")), Cost = 9000, ImagePath = "Images/kia_rio.jpg" },
                new Vehicle { Id = 9, Name = "Ford Explorer", Description = "Family-sized SUV", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("suv")), Cost = 29000, ImagePath = "Images/explorer.jpg" },
                new Vehicle { Id = 10, Name = "Shelby Cobra", Description = "British retro sport car", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("cabrio")), Cost = 55000, ImagePath = "Images/shelby.jpg" },
                new Vehicle { Id = 11, Name = "Reanult Twingo", Description = "Small french hatch-back for convenient city transportation", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("hatch-back")), Cost = 14000, ImagePath = "Images/twingo.jpg" },
                new Vehicle { Id = 12, Name = "Volskwagen Polo", Description = "Profitable european sedan", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("sedan")), Cost = 8000, ImagePath = "Images/polo.jpg" },
                new Vehicle { Id = 13, Name = "Renault Logan", Description = "я больше не могу придумать описание", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("sedan")), Cost = 8000, ImagePath = "Images/logan.jpg" },
                new Vehicle { Id = 13, Name = "Dodge RAM", Description = "American heavy pickup with great acceleration", Type = vehicleTypes.Find(v=>v.NormalizedName.Equals("pickup")), Cost = 28000, ImagePath = "Images/ram.jpg" },
            };
        }

        public Task<ResponseData<Vehicle>> CreateProductAsync(Vehicle product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Vehicle>> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Vehicle>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Vehicle>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1)
        {
            var listModel = new ListModel<Vehicle>();
            if (categoryNormalizedName != null)
                listModel.Items = _vehicles.Where(t => t.Type.NormalizedName == categoryNormalizedName).ToList();
            else
                listModel.Items = _vehicles;

            int itemsPerPage = Convert.ToInt32(_configuration["ItemsPerPage"]);

            int totalPages = (int)Math.Ceiling(
                (double)listModel.Items.Count / itemsPerPage
                );
            
            listModel.TotalPages = totalPages;
            
            if (pageNo > totalPages || pageNo < 1)
                return Task.FromResult(
                    ResponseData<ListModel<Vehicle>>
                    .Error("Index out of bounds")
                    );

            int currentPage = pageNo;
            listModel.CurrentPage = currentPage;
            listModel.Items = listModel.Items.Skip(itemsPerPage * (currentPage - 1)).Take(itemsPerPage).ToList();

            var result = ResponseData<ListModel<Vehicle>>.Success(listModel);
            return Task.FromResult(result);
        }

        public Task<ResponseData<Vehicle>> UpdateProductAsync(int id, Vehicle product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
