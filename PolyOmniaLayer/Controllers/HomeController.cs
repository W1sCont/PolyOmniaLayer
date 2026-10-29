using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PolyOmnia.BLL.Interfaces;
using PolyOmnia.BLL.Services;
using PolyOmniaLayer.Models;
using PolyOmniaLayer.Models.Page;
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
        public async Task<IActionResult> Index(
            string? searchQuery,
            int? categoryId,
            int page = 1,
            SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 6;
            var videosQuery = await _videoService.GetApprovedVideos();
            var hero = videosQuery.FirstOrDefault();

            if (categoryId.HasValue)
            {
                videosQuery = videosQuery.Where(v => v.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                videosQuery = videosQuery.Where(v => v.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            videosQuery = sortOrder switch
            {
                SortState.NameAsc => videosQuery.OrderBy(s => s.Title),
                SortState.NameDesc => videosQuery.OrderByDescending(s => s.Title),
                SortState.CategoryAsc => videosQuery.OrderBy(s => s.Category),
                SortState.CategoryDesc => videosQuery.OrderByDescending(s => s.Category),
                _ => videosQuery.OrderBy(s => s.CreatedAt),
            };

            int totalItems = videosQuery.Count();

            var pagedVideos = videosQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var categories = await _categoryService.GetAllCategories();

            var homeModel = new HomeViewModel
            {
                HeroVideo = hero, 
                Videos = pagedVideos,                     
                Category = categories,
                SelectedCategoryId = categoryId
            };

            var pageModel = new PageViewModel(totalItems, page, pageSize);
            var filter = new FilterViewModel(categories, categoryId, searchQuery);
            var sort = new SortViewModel(sortOrder);

            var model = new IndexViewModel(homeModel, pageModel, filter, sort);

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
