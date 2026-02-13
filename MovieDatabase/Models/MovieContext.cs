using Microsoft.EntityFrameworkCore;

namespace MovieDatabase.Models
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>().HasData(
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
