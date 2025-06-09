using BIL551_LanguageLearningPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BIL551_LanguageLearningPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected string CurrentUserRole => HttpContext.Session.GetString("Role");
        protected int CurrentUserID => int.Parse(HttpContext.Session.GetString("UserID"));

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            return View();
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
