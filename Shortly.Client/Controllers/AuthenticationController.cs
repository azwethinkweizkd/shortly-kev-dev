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

        public IActionResult LoginSubmitted(LoginVm loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginVM);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View(new RegisterVm());
        }

        public IActionResult RegisterUser(RegisterVm registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", registerVM);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}