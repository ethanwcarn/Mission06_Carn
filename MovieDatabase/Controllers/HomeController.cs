// HomeController.cs - Handles requests for the main informational pages of the site.
// Serves the Home page, the "Get to Know Joel" page, and the error page.

using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models;
using System.Diagnostics;

namespace MovieDatabase.Controllers
{
    // Controller for non-movie pages (Home, About Joel, Error)
    public class HomeController : Controller
    {
        // GET: /Home/Index - Displays the home page with the site heading and Joel's image
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Joel - Displays the "Get to Know Joel" page with links to his comedy and podcast
        public IActionResult Joel()
        {
            return View();
        }

        // GET: /Home/Error - Displays a generic error page with the current request ID.
        // ResponseCache is disabled so error pages are never cached.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
