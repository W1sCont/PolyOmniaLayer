using Microsoft.AspNetCore.Mvc;
using PolyOmnia.BLL.DTO;
using PolyOmnia.BLL.Interfaces;
using PolyOmniaLayer.Models;

namespace PolyOmniaLayer.Controllers
{
    public class AdminController(IUserService userService, IVideoService videoService, ICategoryService categoryService) : Controller
    {
        // Category
        public async Task<IActionResult> Categories()
        {
            var category = await categoryService.GetAllCategories();
            return View(category);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await categoryService.CreateCategory(model);
            TempData["SuccessMessage"] = "Категорію успішно додано!";

            return RedirectToAction(nameof(Categories));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await categoryService.GetCategoryById(id);
            if (category == null) return NotFound();

            return View("CreateCategory", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var category = await categoryService.GetCategoryById(model.Id);
            if (category == null) return NotFound();

            category.Name = model.Name.Trim();
            await categoryService.UpdateCategory(category);

            TempData["SuccessMessage"] = "Категорію успішно оновлено!";
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryService.GetCategoryById(id);
            if (category != null)
            {
                await categoryService.DeleteCategory(category);
                TempData["SuccessMessage"] = "Категорію успішно видалено!";
            }

            return RedirectToAction(nameof(Categories));
        }

        // User
        public async Task<IActionResult> ApproveUsers(){
            var users = await userService.GetUsersByRole("Unconfirmed");
            var model = users.Select(u => new AdminUserItemViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Login = u.Login,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveUser(int id)
        {
            await userService.UpdateUserRole(id, "User");
            return RedirectToAction(nameof(ApproveUsers));
        }

        public async Task<IActionResult> RejectUser(int id)
        {
            await userService.DeleteUser(id);
            return RedirectToAction(nameof(ApproveUsers));
        }

        // Video
        public async Task<IActionResult> ApproveVideos()
        {
            var videos = await videoService.GetPendingVideosAsync();
            var model = videos.Select(v => new AdminVideoItemViewModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    UserName = v.User != null ? v.User : "Unknown",
                    Description = v.Description,
                    ThumbnailUrlOrPath = v.ThumbnailUrlOrPath,
                    Duration = v.Duration,
                    Category = v.Category != null 
                        ? v.Category
                        : "No category",
                    }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveVideo(int id)
        {
            await videoService.ApproveVideoAsync(id);
            return RedirectToAction(nameof(ApproveVideos));
        }

        public async Task<IActionResult> RejectVideo(int id)
        {
            await videoService.DeleteVideoById(id);
            return RedirectToAction(nameof(ApproveVideos));
        }
    }
}
