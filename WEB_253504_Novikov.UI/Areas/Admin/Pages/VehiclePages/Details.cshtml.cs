using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.UI.Services.VehicleService;
using WEB_253504_Novikov.UI.Services.VehicleTypeService;

namespace WEB_253504_Novikov.UI.Areas.Admin.Pages.VehiclePages
{
    public class DetailsModel : PageModel
    {
        private readonly IVehicleService _context;
        private readonly IVehicleTypeService _vehicleTypeService ;

        public DetailsModel(IVehicleService context, IVehicleTypeService vehicleTypeService)
        {
            _context = context;
            _vehicleTypeService = vehicleTypeService;
        }

        public VehicleType Type { get; set; }
        public Vehicle Vehicle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var resultType = await _vehicleTypeService.GetCategoryListAsync();
            var resultVehicle = await _context.GetProductByIdAsync(id ?? 0);
            if (resultVehicle.Successfull && resultType.Successfull)
            {
                Vehicle = resultVehicle.Data;
                Type = resultType.Data.FirstOrDefault(t => t.Id == Vehicle.TypeId);
            }

            /*var vehicle = await _context.Vehicle.FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            else
            {
                Vehicle = vehicle;
            }*/
            return Page();
        }
    }
}
