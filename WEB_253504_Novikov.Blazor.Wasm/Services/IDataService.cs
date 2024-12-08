using WEB_253504_Novikov.Domain.Entities;

namespace WEB_253504_Novikov.Blazor.Wasm.Services
{
    public interface IDataService
    {
        event Action DataLoaded;

        List<VehicleType> VehicleTypes { get; set; }

        List<Vehicle> Vehicles { get; set; }

        bool Success { get; set; }

        string ErrorMessage { get; set; }

        int TotalPages { get; set; }

        int CurrentPage { get; set; }

        VehicleType? SelectedVehicleType { get; set; }

        Task GetProductListAsync(int pageNo = 1);

        Task GetCategoryListAsync();
    }
}
