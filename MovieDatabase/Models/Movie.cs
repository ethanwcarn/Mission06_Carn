using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models
{
    // Represents a single movie in the Joel Hilton Film Collection.
    // Each movie belongs to one Category via the CategoryId foreign key.
    public class Movie
    {
        // Primary key
        public int Id { get; set; }

        // Foreign key linking to the Category table
        [Required]
        public int CategoryId { get; set; }

        // Navigation property for the related Category
        public Category? Category { get; set; }

        // Title of the movie
        [Required]
        public string Title { get; set; } = string.Empty;

        // Release year, constrained to valid film history range
        [Required]
        [Range(1888, 2100, ErrorMessage = "Year must be between 1888 and 2100.")]
        public int Year { get; set; }

        // Director(s) of the movie
        [Required]
        public string Director { get; set; } = string.Empty;

        // MPAA rating (G, PG, PG-13, R, etc.)
        [Required]
        public string Rating { get; set; } = string.Empty;

        // Whether the movie has been edited for content (null = unknown)
        public bool? Edited { get; set; }

        // Name of the person the movie is currently lent to, if any
        public string? LentTo { get; set; }

        // Short notes about the movie (max 25 characters)
        [StringLength(25, ErrorMessage = "Notes must be 25 characters or fewer.")]
        public string? Notes { get; set; }
    }
}
