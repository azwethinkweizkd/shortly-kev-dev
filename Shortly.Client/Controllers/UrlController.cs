using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortly.Client.Data.ViewModels;
using Shortly.Client.Helpers.Roles;
using Shortly.Data;
using Shortly.Data.Models;
using Shortly.Data.Services;
using System.Security.Claims;

namespace Shortly.Client.Controllers
{
    public class UrlController(IUrlsService urlsService, IMapper mapper) : Controller
    {
        private readonly IUrlsService _urlsService = urlsService;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> Index()
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(Role.Admin);

            var allUrls = await urlsService
                .GetUrlsAsync(loggedInUserId, isAdmin);
            var mappedAllUrls = mapper.Map<List<Url>, List<GetUrlVm>>(allUrls);
                

            return View(mappedAllUrls);
        }

        public IActionResult Create() {


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            await urlsService.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
