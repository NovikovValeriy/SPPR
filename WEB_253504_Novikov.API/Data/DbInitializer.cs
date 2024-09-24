using Microsoft.EntityFrameworkCore;
using WEB_253504_Novikov.Domain.Entities;

namespace WEB_253504_Novikov.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var route = app.Configuration.GetSection("ImagePath").Value;
            var vehicleTypes = new List<VehicleType>
            {
                new VehicleType{ Id = 1, Name = "Sedan", NormalizedName = "sedan"},
                new VehicleType{ Id = 2, Name = "SUV", NormalizedName = "suv"},
                new VehicleType{ Id = 3, Name = "Pickup", NormalizedName = "pickup"},
                new VehicleType{ Id = 4, Name = "Coupe", NormalizedName = "coupe"},
                new VehicleType{ Id = 5, Name = "Hatch Back", NormalizedName = "hatch-back"},
                new VehicleType{ Id = 6, Name = "Cabrio", NormalizedName = "cabrio"},
            };
            var _vehicles = new List<Vehicle>
            {
                new Vehicle { Name = "Ford Crown Victoria", Description = "American sedan frequently used by police", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("sedan")), Cost = 5000, ImagePath = route + "crown_victoria.jpg"},
                new Vehicle { Name = "Ford F150", Description = "Popular utility truck", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("pickup")), Cost = 25000, ImagePath = route + "f150.jpg" },
                new Vehicle { Name = "Cadillac Escalade", Description = "American luxury SUV", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("suv")), Cost = 32000, ImagePath = route + "escalade.jpg" },
                new Vehicle { Name = "Mercedes McLaren", Description = "Luxury coupe", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("coupe")), Cost = 120000, ImagePath = route + "mclaren.jpg" },
                new Vehicle { Name = "Lada Granta", Description = "Russian inexpensive sedan", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("sedan")), Cost = 3000, ImagePath = route + "granta.jpg" },
                new Vehicle { Name = "Suzuki Swift", Description = "Small and quick korean hatch-back", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("hatch-back")), Cost = 8000, ImagePath = route + "swift.jpg" },
                new Vehicle { Name = "Toyota Hilux", Description = "A reliant utility vehicle", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("pickup")), Cost = 12000, ImagePath = route + "hilux.jpg" },
                new Vehicle { Name = "Kia Rio", Description = "Popular korean sedan", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("sedan")), Cost = 9000, ImagePath = route + "kia_rio.jpg" },
                new Vehicle { Name = "Ford Explorer", Description = "Family-sized SUV", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("suv")), Cost = 29000, ImagePath = route + "explorer.jpg" },
                new Vehicle { Name = "Shelby Cobra", Description = "British retro sport car", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("cabrio")), Cost = 55000, ImagePath = route + "shelby.jpg" },
                new Vehicle { Name = "Reanult Twingo", Description = "Small french hatch-back for convenient city transportation", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("hatch-back")), Cost = 14000, ImagePath = route + "twingo.jpg" },
                new Vehicle { Name = "Volskwagen Polo", Description = "Profitable european sedan", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("sedan")), Cost = 8000, ImagePath = route + "polo.jpg" },
                new Vehicle { Name = "Renault Logan", Description = "я больше не могу придумать описание", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("sedan")), Cost = 8000, ImagePath = route + "logan.jpg" },
                new Vehicle { Name = "Dodge RAM", Description = "American heavy pickup with great acceleration", Type = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("pickup")), Cost = 28000, ImagePath = route + "ram.jpg" },
            };
            await dbContext.VehicleTypes.AddRangeAsync(vehicleTypes);
            await dbContext.Vehicles.AddRangeAsync(_vehicles);
            await dbContext.SaveChangesAsync();
        }
    }
}
