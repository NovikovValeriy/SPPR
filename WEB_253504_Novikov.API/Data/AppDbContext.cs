using Microsoft.EntityFrameworkCore;
using WEB_253504_Novikov.Domain.Entities;

namespace WEB_253504_Novikov.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
    }
}
