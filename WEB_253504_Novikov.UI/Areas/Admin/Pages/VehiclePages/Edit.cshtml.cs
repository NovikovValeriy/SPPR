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
    public class EditModel : PageModel
    {
        private readonly IVehicleService _service;
        private readonly IVehicleTypeService _vehicleTypeService;

        public EditModel(IVehicleService context, IVehicleTypeService vehicleTypeService)
        {
            _service = context;
            _vehicleTypeService = vehicleTypeService;
        }

        [BindProperty]
        public Vehicle Vehicle { get; set; } = default!;
        public List<VehicleType> VehicleTypes { get; set; }

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = await _service.GetProductByIdAsync(id ?? default);
            if (!result.Successfull)
            {
                return NotFound();
            }
            Vehicle = result.Data;
            var resultTypes = await _vehicleTypeService.GetCategoryListAsync();
            if (!resultTypes.Successfull)
            {
                return NotFound();
            }
            VehicleTypes = resultTypes.Data;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _service.UpdateProductAsync(Vehicle.Id, Vehicle, Image);
            if (!result.Successfull)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }

        private bool VehicleExists(int id)
        {
            //return _context.Vehicle.Any(e => e.Id == id);
            return false;
        }
    }
}
