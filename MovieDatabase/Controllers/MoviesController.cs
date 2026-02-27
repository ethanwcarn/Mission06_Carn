// MoviesController.cs - Handles all CRUD operations for movies in the database.
// Provides actions to list, add, edit, and delete movies from Joel's film collection.

using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models;

namespace MovieDatabase.Controllers
{
    // Controller responsible for movie-related pages and database operations
    public class MoviesController : Controller
    {
        // Database context injected via constructor dependency injection
        private readonly MovieContext _context;

        // Constructor receives the MovieContext from the DI container
        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET: /Movies/Index - Retrieves all movies from the database, sorted alphabetically by title,
        // and displays them in a table view
        [HttpGet]
        public IActionResult Index()
        {
            var movies = _context.Movies
                .OrderBy(m => m.Title)
                .ToList();

            return View(movies);
        }

        // GET: /Movies/Add - Displays the blank form for adding a new movie
        [HttpGet]
        public IActionResult Add()
        {
            return View(new Movie());
        }

        // POST: /Movies/Add - Receives the submitted form data, validates it,
        // saves the new movie to the database, and shows a confirmation page.
        // ValidateAntiForgeryToken protects against cross-site request forgery attacks.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Movie movie)
        {
            // If validation fails, redisplay the form with error messages
            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            // Add the new movie to the database and save changes
            _context.Movies.Add(movie);
            _context.SaveChanges();

            // Show the confirmation page with the saved movie details
            return View("Confirmation", movie);
        }

        // GET: /Movies/Edit/{id} - Looks up a movie by ID and displays the edit form pre-filled with its data
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Find(id);

            // Return 404 if the movie doesn't exist
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: /Movies/Edit/{id} - Receives updated movie data from the edit form,
        // validates it, saves changes to the database, and redirects to the movie list
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Movie movie)
        {
            // Ensure the URL id matches the form's movie id to prevent tampering
            if (id != movie.Id)
            {
                return BadRequest();
            }

            // If validation fails, redisplay the form with error messages
            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            // Update the existing movie record in the database
            _context.Update(movie);
            _context.SaveChanges();

            // Redirect back to the full movie list after successful edit
            return RedirectToAction(nameof(Index));
        }

        // GET: /Movies/Delete/{id} - Displays a confirmation page asking if the user wants to delete the movie
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);

            // Return 404 if the movie doesn't exist
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: /Movies/Delete/{id} - Removes the movie from the database after the user confirms deletion.
        // ActionName("Delete") maps this method to the same URL as the GET Delete action.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.Find(id);

            // Return 404 if the movie doesn't exist
            if (movie == null)
            {
                return NotFound();
            }

            // Remove the movie from the database and save changes
            _context.Movies.Remove(movie);
            _context.SaveChanges();

            // Redirect back to the movie list after successful deletion
            return RedirectToAction(nameof(Index));
        }
    }
}
