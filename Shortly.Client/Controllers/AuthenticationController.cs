using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortly.Client.Data.ViewModels;
using Shortly.Client.Helpers.Roles;
using Shortly.Data;
using Shortly.Data.Models;
using Shortly.Data.Services;

namespace Shortly.Client.Controllers
{
    public class AuthenticationController(IUsersService usersService,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager) : Controller
    {
        private readonly IUsersService _usersService = usersService;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<IActionResult> Users()
        {
            var users = await _usersService.GetUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> Login()
        {
            return View(new LoginVm());
        }

        public async Task<IActionResult> HandleLogin(LoginVm loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginVM);
            }

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var userPasswordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (userPasswordCheck)
                {
                    var userLoggedIn = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                    if (userLoggedIn.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    } else if (userLoggedIn.IsNotAllowed)
                    {
                       
                        return RedirectToAction("EmailConfirmation");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt. Please, check your username and password");
                        return View("Login", loginVM);
                    }
                }
            } else
            {
                await _userManager.AccessFailedAsync(user);

                if(await _userManager.IsLockedOutAsync(user))
                {
                    ModelState.AddModelError("", "Your account is locked, please try again in 10 mins");
                    return View("Login", loginVM);
                }

                ModelState.AddModelError("", "Invalid login attempt. Please, check your username and password");
                return View("Login", loginVM);
            }


            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register()
        {
            return View(new RegisterVm());
        }

        public async Task<IActionResult> RegisterUser(RegisterVm registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", registerVM);
            }

            var userEmail = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if(userEmail != null)
            {
                ModelState.AddModelError("", "Email address is already in use.");
            }

            var userUsername = await _userManager.FindByNameAsync(registerVM.Username);
            if (userUsername != null)
            {
                ModelState.AddModelError("", "Username is already in use.");
            }

            var newUser = new AppUser()
            {
                Email = registerVM.EmailAddress,
                UserName = registerVM.Username,
                FullName = registerVM.FullName,
                LockoutEnabled = true
            };

            var userCreated = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (userCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, Role.User);

                //Login user

                await _signInManager.PasswordSignInAsync(newUser, registerVM.Password, false, false);
            } else
            {
                foreach (var error in userCreated.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Register", registerVM);
            };

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> EmailConfirmation()
        {
            var confirmEmail = new ConfirmEmailLoginVm();
            return View(confirmEmail);
        }
    }
}