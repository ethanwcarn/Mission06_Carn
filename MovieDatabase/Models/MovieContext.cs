using Microsoft.EntityFrameworkCore;

namespace MovieDatabase.Models
{
    // Entity Framework database context for the Movie application.
    // Manages the Movies and Categories tables and seeds initial category data.
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        // Table of all movies in the collection
        public DbSet<Movie> Movies { get; set; } = null!;

        // Table of movie categories/genres (e.g. Comedy, Drama)
        public DbSet<Category> Categories { get; set; } = null!;

        // Seeds the Categories table with the standard genre list on first migration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pre-populate categories so the dropdown is never empty
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Action/Adventure" },
                new Category { CategoryId = 2, Name = "Comedy" },
                new Category { CategoryId = 3, Name = "Drama" },
                new Category { CategoryId = 4, Name = "Family" },
                new Category { CategoryId = 5, Name = "Horror/Suspense" },
                new Category { CategoryId = 6, Name = "Miscellaneous" },
                new Category { CategoryId = 7, Name = "Television" },
                new Category { CategoryId = 8, Name = "VHS" }
            );
        }
    }
}
