using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            return View(statusCode);
        }

        [HttpGet]
        public IActionResult ErrorMessage(string message)
        {
            return View(message);
        }

    }
}
