using CsvHelper.Configuration.Attributes;

namespace MovieDatabase.Models;

// Data Transfer Object (DTO) for mapping Joel Hilton Movie Collection CSV columns.
// Each property maps to a column by zero-based index.
public class MovieCsvRow
{
    [Index(0)]
    public int Id { get; set; }

    // Category name as a raw string from the CSV (mapped to CategoryId during import)
    [Index(1)]
    public string Category { get; set; } = string.Empty;

    [Index(2)]
    public string Title { get; set; } = string.Empty;

    // Year can be a range like "2001-2002"; parsed in Program.cs
    [Index(3)]
    public string Year { get; set; } = string.Empty;

    [Index(4)]
    public string Director { get; set; } = string.Empty;

    [Index(5)]
    public string Rating { get; set; } = string.Empty;

    // "Yes" or blank in the CSV; converted to bool? during import
    [Index(6)]
    public string? Edited { get; set; }

    [Index(7)]
    public string? LentTo { get; set; }

    [Index(8)]
    public string? Notes { get; set; }
}
