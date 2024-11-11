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
                new VehicleType{ Name = "Sedan", NormalizedName = "sedan", Id = 1 },
                new VehicleType{ Name = "SUV", NormalizedName = "suv", Id = 2},
                new VehicleType{ Name = "Pickup", NormalizedName = "pickup", Id = 3},
                new VehicleType{ Name = "Coupe", NormalizedName = "coupe", Id = 4},
                new VehicleType{ Name = "Hatch Back", NormalizedName = "hatch-back", Id = 5},
                new VehicleType{ Name = "Cabrio", NormalizedName = "cabrio", Id = 6},
            };
            var _vehicles = new List<Vehicle>
            {
                new Vehicle { Name = "Ford Crown Victoria", Description = "American sedan frequently used by police", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("sedan")).Id, Cost = 5000, ImagePath = route + "crown_victoria.jpg"},
                new Vehicle { Name = "Ford F150", Description = "Popular utility truck", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("pickup")).Id, Cost = 25000, ImagePath = route + "f150.jpg" },
                new Vehicle { Name = "Cadillac Escalade", Description = "American luxury SUV", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("suv")).Id, Cost = 32000, ImagePath = route + "escalade.jpg" },
                new Vehicle { Name = "Mercedes McLaren", Description = "Luxury coupe", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("coupe")).Id, Cost = 120000, ImagePath = route + "mclaren.jpg" },
                new Vehicle { Name = "Lada Granta", Description = "Russian inexpensive sedan", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("sedan")).Id, Cost = 3000, ImagePath = route + "granta.jpg" },
                new Vehicle { Name = "Suzuki Swift", Description = "Small and quick korean hatch-back", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("hatch-back")).Id, Cost = 8000, ImagePath = route + "swift.jpg" },
                new Vehicle { Name = "Toyota Hilux", Description = "A reliant utility vehicle", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("pickup")).Id, Cost = 12000, ImagePath = route + "hilux.jpg" },
                new Vehicle { Name = "Kia Rio", Description = "Popular korean sedan", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("sedan")).Id, Cost = 9000, ImagePath = route + "kia_rio.jpg" },
                new Vehicle { Name = "Ford Explorer", Description = "Family-sized SUV", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("suv")).Id, Cost = 29000, ImagePath = route + "explorer.jpg" },
                new Vehicle { Name = "Shelby Cobra", Description = "British retro sport car", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("cabrio")).Id, Cost = 55000, ImagePath = route + "shelby.jpg" },
                new Vehicle { Name = "Reanult Twingo", Description = "Small french hatch-back for convenient city transportation", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("hatch-back")).Id, Cost = 14000, ImagePath = route + "twingo.jpg" },
                new Vehicle { Name = "Volskwagen Polo", Description = "Profitable european sedan", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("sedan")).Id, Cost = 8000, ImagePath = route + "polo.jpg" },
                new Vehicle { Name = "Renault Logan", Description = "я больше не могу придумать описание", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("sedan")).Id, Cost = 8000, ImagePath = route + "logan.jpg" },
                new Vehicle { Name = "Dodge RAM", Description = "American heavy pickup with great acceleration", TypeId = vehicleTypes.FirstOrDefault(v=>v.NormalizedName.Equals("pickup")).Id, Cost = 28000, ImagePath = route + "ram.jpg" },
            };
            await dbContext.VehicleTypes.AddRangeAsync(vehicleTypes);
            await dbContext.Vehicles.AddRangeAsync(_vehicles);
            await dbContext.SaveChangesAsync();
        }
    }
}
