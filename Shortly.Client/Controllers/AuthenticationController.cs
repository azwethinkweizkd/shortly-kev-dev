using Microsoft.AspNetCore.Mvc;
using Shortly.Client.Data.ViewModels;

namespace Shortly.Client.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Users()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View(new LoginVm());
        }
        public IActionResult HandleLogin(LoginVm loginVm)
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
