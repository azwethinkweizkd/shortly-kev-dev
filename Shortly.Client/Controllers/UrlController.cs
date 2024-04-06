using Microsoft.AspNetCore.Mvc;
using Shortly.Client.Data.Models;

namespace Shortly.Client.Controllers
{
    public class UrlController : Controller
    {
        public IActionResult Index()
        {
            //Fake Db Data
            var allUrls = new List<Url>()
            {
                new Url()
                {
                    Id = 1,
                    OriginalLink = "https://link1.com",
                    ShortLink = "sh1",
                    NumOfClicks = 1,
                    UserId = 1,
                },
                new Url()
                {
                    Id = 2,
                    OriginalLink = "https://link2.com",
                    ShortLink = "sh2",
                    NumOfClicks = 2,
                    UserId = 2,
                },
                new Url()
                {
                    Id = 3,
                    OriginalLink = "https://link3.com",
                    ShortLink = "sh3",
                    NumOfClicks = 3,
                    UserId = 3,
                }
            };

            return View(allUrls);
        }

        public IActionResult Create() {


            return RedirectToAction("Index");
        }
    }
}
