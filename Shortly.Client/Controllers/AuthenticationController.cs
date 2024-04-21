using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using Shortly.Client.Data.ViewModels;
using Shortly.Client.Helpers.Roles;
using Shortly.Data;
using Shortly.Data.Models;
using Shortly.Data.Services;

namespace Shortly.Client.Controllers
{
    public class AuthenticationController(IUsersService usersService,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        IConfiguration configuration) : Controller
    {
        private readonly IUsersService _usersService = usersService;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;

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
                    //else if (userLoggedIn.RequiresTwoFactor)
                    //{
                    //    return RedirectToAction("TwoFactorConfirmation", new { loggedInUserId = user.Id});
                    //}
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

        public async Task<IActionResult> SendEmailConfirmation(ConfirmEmailLoginVm confirmEmailLoginVM)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailLoginVM.EmailAddress);

            if(user != null) 
            {
                var userToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var userConfirmationLink = Url.Action("EmailConfirmationVerified", "Authentication", new { userId = user.Id, userConfirmationToken = userToken }, Request.Scheme);

                //3. Send the email
                var apiKey = _configuration["SendGrid:ShrtlyKey"];
                var sendGridClient = new SendGridClient(apiKey);

                var fromEmailAddress = new EmailAddress(_configuration["SendGrid:FromAddress"], "Shrtly-App");
                var emailSubject = "[Shortly] Verify your account";
                var toEmailAddress = new EmailAddress(confirmEmailLoginVM.EmailAddress);

                var emailContentTxt = $"Hello from Shortly App. Please, click this link to verify your account: {userConfirmationLink}";
                var emailContentHtml = $"Hello from Shortly App. Please, click this link to verify your account: <a href=\"{userConfirmationLink}\"> Verify your account </a> ";

                var emailRequest = MailHelper.CreateSingleEmail(fromEmailAddress, toEmailAddress, emailSubject, emailContentTxt, emailContentHtml);
                var emailResponse = await sendGridClient.SendEmailAsync(emailRequest);
                if (emailResponse.IsSuccessStatusCode)
                {
                    TempData["EmailConfirmation"] = "Thank you! Please, check your email to verify your account";

                    return RedirectToAction("Index", "Home");
                } else
                {
                    ModelState.AddModelError("", $"Confirmation email failed to send");
                }
            } else
            {
                ModelState.AddModelError("", $"Email address {confirmEmailLoginVM.EmailAddress} does not exist");
            }

            return View("EmailConfirmation", confirmEmailLoginVM);
        }

        public async Task<IActionResult> EmailConfirmationVerified(string userId, string userConfirmationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            await _userManager.ConfirmEmailAsync(user, userConfirmationToken);

            TempData["EmailConfirmationVerified"] = "Thank you! Your account has been confirmed. You can now log in!";
            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> TwoFactorConfirmation(string loggedInUserId)
        //{
        //    var user = await _userManager.FindByIdAsync(loggedInUserId);

        //    if (user != null)
        //    {
        //        var userToken = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");

        //        var confirm2FALoginVm = new Confirm2FALoginVm()
        //        {
        //            UserId = loggedInUserId,
        //        };

        //        return View(confirm2FALoginVm);
        //    }
        //    return RedirectToAction("Index", "Home");
        //}
    }
}