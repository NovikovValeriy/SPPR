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
            return View(new ListDemo { Id = 1, Name = "Name1" });
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
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

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
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

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
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
