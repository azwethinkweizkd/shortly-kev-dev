using Microsoft.AspNetCore.Mvc;
using Shortly.Client.Data.ViewModels;
using Shortly.Data;
using Shortly.Data.Models;
using System.Diagnostics;

namespace Shortly.Client.Controllers
{
    public class HomeController(ILogger<HomeController> logger, AppDbContext context) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly AppDbContext _context = context;

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

            var newUrl = new Url()
            {
                OriginalLink = postUrlVM.Url,
                ShortLink = GenerateShortUrl(6),
                NumOfClicks = 0,
                UserId = null,
                DateCreated = DateTime.UtcNow,
            };

            _context.Urls.Add(newUrl);
            _context.SaveChanges();

            TempData["Message"] = $"Your url was shorted successfully to {newUrl.ShortLink}";

            return RedirectToAction("Index");
        }

        private static string GenerateShortUrl(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy0123456789";

            return new string(
                Enumerable.Repeat(chars, length)
                .Select(s => 
                    s[random.Next(s.Length)]
                ).ToArray()
            );
        }
    }
}