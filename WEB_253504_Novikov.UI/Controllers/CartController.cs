using Microsoft.AspNetCore.Mvc;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.UI.Services.VehicleService;

namespace WEB_253504_Novikov.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly Cart _cart;

        public CartController(IVehicleService vehicleService, Cart cart)
        {
            _vehicleService = vehicleService;
            _cart = cart;
        }

        public async Task<IActionResult> Add(int id, string returnUrl)
        {
            var data = await _vehicleService.GetProductByIdAsync(id);

            if (data != null && data.Successfull && data.Data != null)
            {
                _cart.AddToCart(data.Data);
            }

            return Redirect(returnUrl);
        }

        public IActionResult Remove(int id, string returnUrl)
        {
            _cart.RemoveItems(id);
            return Redirect(returnUrl);
        }

        public IActionResult Clear(string returnUrl)
        {
            _cart.ClearAll();
            return Redirect(returnUrl);
        }

        public IActionResult ViewCart()
        {
            return View(_cart);
        }
    }
}
