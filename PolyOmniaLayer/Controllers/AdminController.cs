using Microsoft.AspNetCore.Mvc;
using PolyOmnia.BLL.Interfaces;

namespace PolyOmniaLayer.Controllers
{
    public class AdminController(IUserService userService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
