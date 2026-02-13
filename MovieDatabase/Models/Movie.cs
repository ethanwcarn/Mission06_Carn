using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(1888, 2100, ErrorMessage = "Year must be between 1888 and 2100.")]
        public int Year { get; set; }

        [Required]
        public string Director { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(G|PG|PG-13|R)$", ErrorMessage = "Rating must be G, PG, PG-13, or R.")]
        public string Rating { get; set; } = string.Empty;

        // Optional fields
        public bool? Edited { get; set; }

        public string? LentTo { get; set; }

        [StringLength(25, ErrorMessage = "Notes must be 25 characters or fewer.")]
        public string? Notes { get; set; }
    }
}
