using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models
{
    // Represents a movie genre/category stored in its own database table.
    // Movies reference this via a foreign key (CategoryId).
    public class Category
    {
        // Primary key for the Category table
        public int CategoryId { get; set; }

        // Display name for the category (e.g. "Comedy", "Drama")
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
