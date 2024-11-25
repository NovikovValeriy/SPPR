using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253504_Novikov.UI.Models;

namespace WEB_253504_Novikov.UI.Controllers
{
    public class Home : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewData["Message"] = "Лабораторная работа №2";
            List<ListDemo> lst = new List<ListDemo>
            {
                new ListDemo { Id = 1, Name = "Name1" },
                new ListDemo { Id = 2, Name = "Name2" },
                new ListDemo { Id = 3, Name = "Name3" }
            };
            SelectList selectList = new SelectList(lst, "Id", "Name");
            ViewData["Items"] = selectList;
            return View();
        }
    }
}
