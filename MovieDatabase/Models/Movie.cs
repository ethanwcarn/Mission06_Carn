// Movie.cs - Defines the Movie entity model used by Entity Framework Core.
// Each property maps to a column in the Movies table of the SQLite database.
// Data annotations provide server-side validation rules for form input.

using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models
{
    // Represents a single movie in Joel Hilton's film collection
    public class Movie
    {
        // Primary key - auto-incremented by the database
        public int Id { get; set; }

        // Genre/category of the movie (e.g., Comedy, Action/Adventure) - required field
        [Required]
        public string Category { get; set; } = string.Empty;

        // Title of the movie - required field
        [Required]
        public string Title { get; set; } = string.Empty;

        // Release year of the movie - must be between 1888 (first film ever) and 2100
        [Required]
        [Range(1888, 2100, ErrorMessage = "Year must be between 1888 and 2100.")]
        public int Year { get; set; }

        // Director of the movie - required field
        [Required]
        public string Director { get; set; } = string.Empty;

        // MPAA rating - restricted to G, PG, PG-13, or R via regex validation
        [Required]
        [RegularExpression("^(G|PG|PG-13|R)$", ErrorMessage = "Rating must be G, PG, PG-13, or R.")]
        public string Rating { get; set; } = string.Empty;

        // Whether the movie has been edited for content - nullable boolean (true/false/unknown)
        public bool? Edited { get; set; }

        // Name of the person this movie is currently lent to - optional
        public string? LentTo { get; set; }

        // Short notes about the movie - optional, max 25 characters
        [StringLength(25, ErrorMessage = "Notes must be 25 characters or fewer.")]
        public string? Notes { get; set; }
    }
}
