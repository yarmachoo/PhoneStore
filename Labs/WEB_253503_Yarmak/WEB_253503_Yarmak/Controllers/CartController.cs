using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB_253503_Yarmak.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Add(int id)
        {
            var request = HttpContext.Request;
            var returnUrl = request.Path + request.QueryString.ToUriComponent();
            return View();
        }
    }
}
