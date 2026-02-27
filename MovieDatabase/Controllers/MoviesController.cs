using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Models;

namespace MovieDatabase.Controllers
{
    // Handles all CRUD operations for movies: listing, adding, editing, and deleting.
    public class MoviesController : Controller
    {
        // Database context injected via constructor
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET: /Movies/ — displays all movies ordered alphabetically by title
        [HttpGet]
        public IActionResult Index()
        {
            // Include Category so we can display the category name in the list
            var movies = _context.Movies
                .Include(m => m.Category)
                .OrderBy(m => m.Title)
                .ToList();

            return View(movies);
        }

        // GET: /Movies/Add — shows the blank Add Movie form
        [HttpGet]
        public IActionResult Add()
        {
            // Populate the ViewBag with all categories for the dropdown
            ViewBag.Categories = _context.Categories
                .OrderBy(c => c.Name)
                .ToList();

            return View(new Movie());
        }

        // POST: /Movies/Add — validates and saves a new movie to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate the dropdown if validation fails
                ViewBag.Categories = _context.Categories
                    .OrderBy(c => c.Name)
                    .ToList();
                return View(movie);
            }

            // Add the new movie and persist to the database
            _context.Movies.Add(movie);
            _context.SaveChanges();

            return View("Confirmation", movie);
        }

        // GET: /Movies/Edit/5 — loads the Edit form for a specific movie by ID
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            // Populate the category dropdown for the edit form
            ViewBag.Categories = _context.Categories
                .OrderBy(c => c.Name)
                .ToList();

            return View(movie);
        }

        // POST: /Movies/Edit/5 — validates and updates an existing movie in the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Movie movie)
        {
            // Ensure the route ID matches the form data
            if (id != movie.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // Re-populate the dropdown if validation fails
                ViewBag.Categories = _context.Categories
                    .OrderBy(c => c.Name)
                    .ToList();
                return View(movie);
            }

            // Update the movie record and save changes
            _context.Update(movie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Movies/Delete/5 — shows the delete confirmation page for a specific movie
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Include Category so the confirmation page can display the category name
            var movie = _context.Movies
                .Include(m => m.Category)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: /Movies/Delete/5 — permanently removes the movie from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            // Remove the movie and persist the change
            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
