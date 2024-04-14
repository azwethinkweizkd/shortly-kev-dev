using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortly.Client.Data.ViewModels;
using Shortly.Data;
using Shortly.Data.Models;
using Shortly.Data.Services;

namespace Shortly.Client.Controllers
{
    public class UrlController(IUrlsService urlsService, IMapper mapper) : Controller
    {
        private readonly IUrlsService _urlsService = urlsService;
        private readonly IMapper _mapper = mapper;

        public IActionResult Index()
        {
            var allUrls = urlsService
                .GetUrls();
            var mappedAllUrls = mapper.Map<List<Url>, List<GetUrlVm>>(allUrls);
                

            return View(mappedAllUrls);
        }

        public IActionResult Create() {


            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            urlsService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
