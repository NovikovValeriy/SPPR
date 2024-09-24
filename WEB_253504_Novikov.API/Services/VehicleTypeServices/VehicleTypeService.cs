using Microsoft.EntityFrameworkCore;
using WEB_253504_Novikov.API.Data;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.Domain.Models;

namespace WEB_253504_Novikov.API.Services.VehicleTypeServices
{
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly AppDbContext _context;

        public VehicleTypeService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseData<List<VehicleType>>> GetCategoryListAsync()
        {
            var categories = await _context.VehicleTypes.ToListAsync();
            return ResponseData<List<VehicleType>>.Success(categories);
        }
    }
}
