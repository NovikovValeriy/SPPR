using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.UI.Services.VehicleService;
using WEB_253504_Novikov.UI.Services.VehicleTypeService;

namespace WEB_253504_Novikov.UI.Areas.Admin.Pages.VehiclePages
{
    public class CreateModel : PageModel
    {
        private readonly IVehicleService _service;
        private readonly IVehicleTypeService _vehicleTypeService;

        public CreateModel(IVehicleService service, IVehicleTypeService vehicleTypeService)
        {
            _service = service;
            _vehicleTypeService = vehicleTypeService;
        }

        public async Task<IActionResult> OnGet()
        {
            var result = await _vehicleTypeService.GetCategoryListAsync();
            if (!result.Successfull)
            {
                return NotFound();
            }
            VehicleTypes = result.Data;
            return Page();
        }

        [BindProperty]
        public Vehicle Vehicle { get; set; } = default!;
        public List<VehicleType> VehicleTypes { get; set; }
        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await _service.CreateProductAsync(Vehicle, Image);

            return RedirectToPage("./Index");
        }
    }
}
