using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PolyOmnia.BLL.DTO;
using PolyOmnia.BLL.Interfaces;
using PolyOmnia.DAL.Entities;
using PolyOmnia.DAL.Enums;
using PolyOmniaLayer.Helper;
using PolyOmniaLayer.Models;
using System.Security.Claims;

namespace PolyOmniaLayer.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoService _videoService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public VideoController(IVideoService videoService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _videoService = videoService;
            _categoryService = categoryService;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategories();
            var model = new CreateVideoViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVideoViewModel model)
        {
            // Валідація залежно від обраного типу
            if (model.SourceType == VideoSourceType.LocalFile && model.VideoFile == null)
            {
                ModelState.AddModelError("VideoFile", "Будь ласка, завантажте відеофайл.");
            }
            else if (model.SourceType == VideoSourceType.ExternalUrl && string.IsNullOrWhiteSpace(model.ExternalUrl))
            {
                ModelState.AddModelError("ExternalUrl", "Будь ласка, вкажіть посилання на відео.");
            }

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategories();

                model = new CreateVideoViewModel
                {
                    Categories = categories.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                };

                return View(model);
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId)) return Unauthorized();

            string finalVideoPath = string.Empty;

            // Визначаємо шлях/URL залежно від типу джерела
            if (model.SourceType == VideoSourceType.LocalFile && model.VideoFile != null)
            {
                finalVideoPath = await SaveFileAsync(model.VideoFile, "uploads/videos");
            }
            else if (model.SourceType == VideoSourceType.ExternalUrl && !string.IsNullOrWhiteSpace(model.ExternalUrl))
            {
                finalVideoPath = VideoUrlHelper.ConvertToEmbedUrl(model.ExternalUrl.Trim());
            }

            string? thumbnailPath = null;
            if (model.ThumbnailFile != null)
            {
                thumbnailPath = await SaveFileAsync(model.ThumbnailFile, "uploads/thumbnails");
            }
            else
            {
                thumbnailPath = VideoUrlHelper.GetThumbnailUrl(finalVideoPath);
            }

            var video = new VideoDTO
            {
                Title = model.Title,
                Description = model.Description,
                CategoryId = model.CategoryId,
                UserId = userId,
                SourceType = model.SourceType,
                UrlOrPath = finalVideoPath,
                ThumbnailUrlOrPath = thumbnailPath,
                AllowDownload = model.AllowDownload,
                CreatedAt = DateTime.UtcNow,
                IsPublished = true,
                IsApproved = false // Відео потребує модерації false
            };

            await _videoService.CreateVideo(video);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Watch(int id)
        {
            var video = await _videoService.GetByIdWithDetails(id);
            if (video == null)
            {
                return NotFound();
            }

            // Збільшуємо лічильник переглядів при відкритті сторінки
            video.ViewCount++;
            await _videoService.UpdateVideo(video);
            return View(video);
        }

        // Вспоміжний метод для збереження файлів у wwwroot
        private async Task<string> SaveFileAsync(IFormFile file, string folderPath)
        {
            string uploadsFolder = Path.Combine(_environment.WebRootPath, folderPath);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Генеруємо унікальне ім'я файлу, щоб уникнути перезапису
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/" + folderPath + "/" + uniqueFileName;
        }
    }
}
