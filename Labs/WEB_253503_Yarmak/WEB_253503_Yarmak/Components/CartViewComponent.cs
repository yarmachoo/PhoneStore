using Microsoft.AspNetCore.Mvc;

namespace WEB_253503_Yarmak.Components
{
    public class CartViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
