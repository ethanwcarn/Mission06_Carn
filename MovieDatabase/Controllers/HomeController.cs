using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models;
using System.Diagnostics;

namespace MovieDatabase.Controllers
{
    // Handles the home/landing pages and the global error page.
    public class HomeController : Controller
    {
        // GET: / — displays the site home page
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Joel — displays the Joel Hilton info page
        public IActionResult Joel()
        {
            return View();
        }

        // Displays a generic error page; caching is disabled so errors are always fresh
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
