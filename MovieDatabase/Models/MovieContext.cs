// MovieContext.cs - Entity Framework Core database context for the Movie Database application.
// This class manages the connection to the SQLite database and provides access to the Movies table.
// It also seeds the database with initial movie data when migrations are applied.

using Microsoft.EntityFrameworkCore;

namespace MovieDatabase.Models
{
    // DbContext subclass that represents a session with the SQLite database
    public class MovieContext : DbContext
    {
        // Constructor accepts configuration options (connection string, provider, etc.)
        // passed via dependency injection from Program.cs
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        // DbSet representing the Movies table - allows CRUD operations on Movie entities
        public DbSet<Movie> Movies { get; set; } = null!;

        // Called when the model is being created; used here to seed initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed the database with three movies so the collection isn't empty on first run
            modelBuilder.Entity<Movie>().HasData(
                // Seed movie 1: Hot Fuzz - a comedy directed by Edgar Wright
                new Movie
                {
                    Id = 1,
                    Category = "Comedy",
                    Title = "Hot Fuzz",
                    Year = 2007,
                    Director = "Edgar Wright",
                    Rating = "R",
                    Edited = true,
                    LentTo = null,
                    Notes = "One of Joel's faves"
                },
                // Seed movie 2: Inception - an action/adventure by Christopher Nolan
                new Movie
                {
                    Id = 2,
                    Category = "Action/Adventure",
                    Title = "Inception",
                    Year = 2010,
                    Director = "Christopher Nolan",
                    Rating = "PG-13",
                    Edited = null,
                    LentTo = null,
                    Notes = "Mind-bending classic"
                },
                // Seed movie 3: Groundhog Day - a comedy directed by Harold Ramis
                new Movie
                {
                    Id = 3,
                    Category = "Comedy",
                    Title = "Groundhog Day",
                    Year = 1993,
                    Director = "Harold Ramis",
                    Rating = "PG",
                    Edited = null,
                    LentTo = null,
                    Notes = "Endless rewatch"
                }
            );
        }
    }
}
