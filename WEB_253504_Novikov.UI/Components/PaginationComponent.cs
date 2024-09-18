using Microsoft.AspNetCore.Mvc;
using WEB_253504_Novikov.Domain.Models;

namespace WEB_253504_Novikov.UI.Components
{
    public class PaginationComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PaginationModel paginationModel)
        {
            return View(paginationModel);
        }
    }
}
