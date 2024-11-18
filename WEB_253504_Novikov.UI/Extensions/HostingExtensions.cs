using WEB_253504_Novikov.UI.Authorization;
using WEB_253504_Novikov.UI.HelperClasses;
using WEB_253504_Novikov.UI.Models;
using WEB_253504_Novikov.UI.Services.FileService;
using WEB_253504_Novikov.UI.Services.TokenService;
using WEB_253504_Novikov.UI.Services.VehicleService;
using WEB_253504_Novikov.UI.Services.VehicleTypeService;

namespace WEB_253504_Novikov.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            //builder.Services.AddScoped<IVehicleTypeService, MemoryVehicleTypeService>();
            //builder.Services.AddScoped<IVehicleService, MemoryVehicleService>();
            var uriData = new UriData();
            var uri = builder.Configuration.GetRequiredSection("UriData:ApiUri").Value;
            uriData.ApiUri = uri;
            builder.Services.AddTransient<IAuthService, KeycloakAuthService>();
            builder.Services.AddHttpClient<IVehicleService, ApiVehicleService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services.AddHttpClient<IVehicleTypeService, ApiVehicleTypeService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt => opt.BaseAddress = new Uri($"{uriData.ApiUri}Files"));
            builder.Services
                .Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));

            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
        }
    }
}
