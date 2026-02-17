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
        public IActionResult Index()
        {
            var movies = _context.Movies
                .OrderBy(m => m.Title)
                .ToList();

            return View(movies);
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            _context.Update(movie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
