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
    public class DeleteModel : PageModel
    {
        private readonly IVehicleService _service;
        private readonly IVehicleTypeService _vehicleTypeService;

        public DeleteModel(IVehicleService service, IVehicleTypeService vehicleTypeService)
        {
            _service = service;
            _vehicleTypeService = vehicleTypeService;
        }

        [BindProperty]
        public Vehicle Vehicle { get; set; } = default!;
        public VehicleType Type { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.GetProductByIdAsync(id ?? default(int));
            if (!result.Successfull)
            {
                return NotFound();
            }
            Vehicle = result.Data;
            var resultType = await _vehicleTypeService.GetCategoryListAsync();
            if (!resultType.Successfull)
            {
                return NotFound();
            }
            Type = resultType.Data.FirstOrDefault(t => t.Id == Vehicle.TypeId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _service.DeleteProductAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
