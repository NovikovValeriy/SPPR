using Microsoft.AspNetCore.Mvc;


namespace WEB_253504_Novikov.UI.Components
{
	public class CartComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
