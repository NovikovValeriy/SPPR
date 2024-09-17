using WEB_253504_Novikov.UI.Services.VehicleService;
using WEB_253504_Novikov.UI.Services.VehicleTypeService;

namespace WEB_253504_Novikov.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IVehicleTypeService, MemoryVehicleTypeService>();
            builder.Services.AddScoped<IVehicleService, MemoryVehicleService>();
        }
    }
}
