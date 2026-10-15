using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PolyOmnia.BLL.Interfaces;
using PolyOmniaLayer.Models;
using System.Diagnostics;

namespace PolyOmniaLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVideoService _videoService;
        private readonly ICategoryService _categoryService;
        public HomeController(IVideoService videoService, ICategoryService categoryService) 
        {
            _videoService = videoService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var video = await _videoService.GetApprovedVideos();
            var model = new HomeViewModel
            {
                HeroVideo = video.FirstOrDefault(),
                Videos = video,
                Category = await _categoryService.GetAllCategories()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                }
            );

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
