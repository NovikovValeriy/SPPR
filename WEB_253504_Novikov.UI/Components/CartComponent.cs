using Microsoft.AspNetCore.Mvc;
using WEB_253504_Novikov.Domain.Entities;


namespace WEB_253504_Novikov.UI.Components
{
	public class CartComponent : ViewComponent
	{
        private readonly Cart _cart;

        public CartComponent(Cart cart)
        {
            _cart = cart;
        }
        public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
