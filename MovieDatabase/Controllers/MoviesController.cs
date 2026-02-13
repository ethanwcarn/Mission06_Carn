using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models;

namespace MovieDatabase.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Movie());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            _context.Movies.Add(movie);
            _context.SaveChanges();

            return View("Confirmation", movie);
        }
    }
}

