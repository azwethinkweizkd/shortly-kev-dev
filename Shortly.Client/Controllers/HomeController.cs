using Microsoft.AspNetCore.Mvc;
using Shortly.Client.Data.ViewModels;
using System.Diagnostics;

namespace Shortly.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var newUrl = new PostUrlVm();
            return View(newUrl);
        }

        public IActionResult ShortenUrl(PostUrlVm postUrlVM)
        {
            //Validate the Model
            if (!ModelState.IsValid)
            {
                return View("Index", postUrlVM);
            }

            return RedirectToAction("Index");
        }
    }
}