using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortly.Client.Data.ViewModels;
using Shortly.Data;
using Shortly.Data.Services;

namespace Shortly.Client.Controllers
{
    public class AuthenticationController(IUsersService usersService) : Controller
    {
        private IUsersService _usersService = usersService;

        public async Task<IActionResult> Users()
        {
            var users = await usersService.GetUsersAsync();

            return View(users);
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