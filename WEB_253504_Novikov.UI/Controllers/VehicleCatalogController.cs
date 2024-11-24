using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.Domain.Models;
using WEB_253504_Novikov.UI.Extensions;
using WEB_253504_Novikov.UI.Services.VehicleService;
using WEB_253504_Novikov.UI.Services.VehicleTypeService;

namespace WEB_253504_Novikov.UI.Controllers
{
    public class VehicleCatalogController : Controller
    {
        IVehicleService _vehicleService;
        IVehicleTypeService _vehicleTypeService;

        public VehicleCatalogController(IVehicleService vehicleService, IVehicleTypeService vehicleTypeService)
        {
            _vehicleService = vehicleService;
            _vehicleTypeService = vehicleTypeService;
        }
        // GET: Vehicle
        public async Task<ActionResult> Index(string? vehicleType, int pageNo = 1)
        {
            var categoryResponse =
                await _vehicleTypeService.GetCategoryListAsync();
            if (!categoryResponse.Successfull)
                return NotFound(categoryResponse.ErrorMessage);

            var categories = categoryResponse.Data;
            var category = new VehicleType();
            if (vehicleType == null)
            {
                ViewData["currentVehicleType"] = "Все";
                ViewData["currentVehicleTypeNormalized"] = null;
            }
            else
            {
                category = categories.Find(t => t.NormalizedName == vehicleType);
                if (category == null)
                    return NotFound("No such vehicle type");
                ViewData["currentVehicleType"] = category.Name;
                ViewData["currentVehicleTypeNormalized"] = category.NormalizedName;
            }

            var productResponse =
                await _vehicleService.GetProductListAsync(category?.NormalizedName, pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);
            
            var catalogModel = new CatalogModel(productResponse, categoryResponse);
            catalogModel.CurrentVehicleTypeNormalizedName = category.NormalizedName;

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/_CatalogPartial.cshtml", catalogModel);
            }

            return View(catalogModel);
        }

        // GET: Vehicle/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Vehicle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Vehicle/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Vehicle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Vehicle/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Vehicle/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
