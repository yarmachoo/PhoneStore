using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using WEB_253503_Yarmak.Authorization;
using WEB_253503_Yarmak.Domain.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Policy;


namespace WEB_253503_Yarmak.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(
            RegisterUserViewModel user,
            [FromServices] IAuthService authService)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    return BadRequest();
                }
                var result = await authService.RegisterUserAsync(
                    user.Email, user.Password, user.Avatar);

                if (result.result)
                {
                    return Redirect(Url.Action("Index", "Home"));
                }
                else return BadRequest(result.ErrorMessage);
            }
            return View(user);
        }


        public async Task Login()
        {
            await HttpContext.ChallengeAsync(
                OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") }
                );
        }
        [HttpPost]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(
                OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index",
                "Home")
                });
        }
    }
}
