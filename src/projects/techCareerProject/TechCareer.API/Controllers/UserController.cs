using Microsoft.AspNetCore.Mvc;

namespace TechCareer.API.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
