using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PolyOmnia.BLL.DTO;
using PolyOmnia.BLL.Interfaces;
using PolyOmniaLayer.Helper;
using PolyOmniaLayer.Models;
using System.Security.Claims;

namespace PolyOmniaLayer.Controllers
{
    public class AccountController(IStringLocalizer<SharedResource> localizer, IUserService userService) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", localizer["Err_PasswordsDoNotMatch"]);
                return View(model);
            }

            if (!await userService.IsLoginUnique(model.Login))
            {
                ModelState.AddModelError("Login", localizer["Err_LoginAlreadyExists"]);
                return View(model);
            }

            if (!await userService.IsEmailUnique(model.Email))
            {
                ModelState.AddModelError("Email", localizer["Err_EmailAlreadyExists"]);
                return View(model);
            }

            UserDTO user  = UserRegToDTO.ConvertToUserDTO(model);
            await userService.CreateUser(user);

            if (await userService.IsEmailUnique(model.Email))
            {
                ModelState.AddModelError("", "Error");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var userDto = await userService.Authenticate(model.EmailOrLogin, model.Password);
            if (userDto == null)
            {
                ModelState.AddModelError("", localizer["Err_InvalidLoginOrEmail"]);
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(ClaimTypes.Name, userDto.Login),
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim(ClaimTypes.Role, userDto.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true });

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // 1. Отримуємо Claim з ID залогіненого користувача
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login");
            }

            var user = await userService.GetUserWithVideos(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserProfileDTO
            {
                UserId = user.Id,
                Username = user.Login,
                Email = user.Email,
                RegisteredAt = user.CreatedAt,
                UserVideos = await userService.GetVideoList(userId)
            };

            return View(model);
        }
    }
}
