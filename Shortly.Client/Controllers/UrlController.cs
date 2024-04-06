using Microsoft.AspNetCore.Mvc;
using Shortly.Client.Data.Models;

namespace Shortly.Client.Controllers
{
    public class UrlController : Controller
    {
        public IActionResult Index()
        {

            ////Data is from DB

            //var urlDb = new Url()
            //{
            //    Id = 1,
            //    OriginalLink = "https://original.com",
            //    ShortLink = "shrtly",
            //    NumOfClicks = 1,
            //    UserId = 1,
            //    DateCreated = DateTime.Now,
            //};

            //var allData = new List<Url>
            //{
            //    urlDb
            //};

            //return View(allData);

            //ViewData["ShortenedUrl"] = "This is just a short url";
            //ViewData["AllUrls"] = new List<string>() { "Url1", "Url2", "Url3" };

            //ViewBag.ShortenedUrl = "This is just a short url";
            //ViewBag.AllUrls = new List<string>() { "Url1", "Url2", "Url3" };

            var tempData = TempData["SuccessMessage"];
            var viewBag = ViewBag.Test1;
            var viewData = ViewData["Test1"];

            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View();
        }

        public IActionResult Create() {
            //ShortenedUrl

            var shortenedUrl = 'short';

            TempData["SuccessMessage"] = "Successful!";
            ViewBag.Test1 = "test1";
            ViewData["Test2"] = "test2";

            return RedirectToAction("Index");
        }
    }
}
