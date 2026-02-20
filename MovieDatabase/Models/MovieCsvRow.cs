using CsvHelper.Configuration.Attributes;

namespace MovieDatabase.Models;

/// <summary>
/// DTO for mapping Joel Hilton Movie Collection CSV columns by index.
/// </summary>
public class MovieCsvRow
{
    [Index(0)]
    public int Id { get; set; }

    [Index(1)]
    public string Category { get; set; } = string.Empty;

    [Index(2)]
    public string Title { get; set; } = string.Empty;

    [Index(3)]
    public string Year { get; set; } = string.Empty;

    [Index(4)]
    public string Director { get; set; } = string.Empty;

    [Index(5)]
    public string Rating { get; set; } = string.Empty;

    [Index(6)]
    public string? Edited { get; set; }

    [Index(7)]
    public string? LentTo { get; set; }

    [Index(8)]
    public string? Notes { get; set; }
}
