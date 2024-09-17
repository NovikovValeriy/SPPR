using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.Domain.Models;

namespace WEB_253504_Novikov.UI.Services.VehicleTypeService
{
    public class MemoryVehicleTypeService : IVehicleTypeService
    {
        public Task<ResponseData<List<VehicleType>>> GetCategoryListAsync()
        {
            var vehicleTypes = new List<VehicleType>
            {
                new VehicleType{ Id = 1, Name = "Sedan", NormalizedName = "sedan"},
                new VehicleType{ Id = 2, Name = "SUV", NormalizedName = "suv"},
                new VehicleType{ Id = 3, Name = "Pickup", NormalizedName = "pickup"},
                new VehicleType{ Id = 4, Name = "Coupe", NormalizedName = "coupe"},
                new VehicleType{ Id = 5, Name = "Hatch Back", NormalizedName = "hatch-back"},
                new VehicleType{ Id = 6, Name = "Cabrio", NormalizedName = "cabrio"},
            };
            var result = ResponseData<List<VehicleType>>.Success(vehicleTypes);
            return Task.FromResult(result);
        }
    }
}
