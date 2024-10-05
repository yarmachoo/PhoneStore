using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_Yarmak.Models;

namespace WEB_253503_Yarmak.Controllers
{
    public class HomeController : Controller
    {
        [ViewData]
        public string LabRab2 {  get; set; }
        IEnumerable<ListDemo> listDemo = new List<ListDemo>
        {
            new ListDemo{Id = 1, Name = "Item 1"},
            new ListDemo{Id = 2, Name = "Item 2"},
            new ListDemo{Id = 3, Name = "Item 3"},
            new ListDemo{Id = 4, Name = "Item 4"}
        };
        public IActionResult Index()
        {
            LabRab2 = "Лабораторная работа 2";
            ViewBag.ListDemo = new SelectList(listDemo, "Id", "Name");
            return View();
        }
    }
}
