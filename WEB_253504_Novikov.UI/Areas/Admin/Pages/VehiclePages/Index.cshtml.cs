using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.Domain.Models;
using WEB_253504_Novikov.UI.Services.VehicleService;
using WEB_253504_Novikov.UI.Services.VehicleTypeService;

namespace WEB_253504_Novikov.UI.Areas.Admin.Pages.VehiclePages
{
    public class IndexModel : PageModel
    {
        private readonly IVehicleService _service;
        private readonly IVehicleTypeService _vehicleTypeService;

        public IndexModel(IVehicleService service, IVehicleTypeService vehicleTypeService)
        {
            _service = service;
            _vehicleTypeService = vehicleTypeService;
        }

        public List<VehicleType> Types { get; set; }
        public ListModel<Vehicle> Vehicles { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int pageNo = 1)
        {
            ResponseData<ListModel<Vehicle>> response;
            response = await _service.GetProductListAsync(null, pageNo);
            if (!response.Successfull)
            {
                return NotFound();
            }
            Vehicles = response.Data;
            var responseTypes = await _vehicleTypeService.GetCategoryListAsync();
            if (!responseTypes.Successfull)
            {
                return NotFound();
            }
            Types = responseTypes.Data;
            return Page();
        }
    }
}
