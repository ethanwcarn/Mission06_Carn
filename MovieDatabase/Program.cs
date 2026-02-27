using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Register MVC services (controllers + views)
builder.Services.AddControllersWithViews();

// Configure Entity Framework Core to use SQLite with the connection string from appsettings.json
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MovieConnection")));

// Set explicit Kestrel ports for HTTPS and HTTP
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(7297, listenOptions => listenOptions.UseHttps());
    options.ListenLocalhost(5298);
});

var app = builder.Build();

// One-time CSV import: loads the Joel Hilton Movie Collection from a CSV file
// into the database on first run (or when the DB has fewer rows than the CSV).
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    var csvPath = Path.Combine(env.ContentRootPath, "Data", "JoelHiltonMovieCollection.csv");

    // Only import if the CSV exists and the database isn't already fully loaded
    if (File.Exists(csvPath) && context.Movies.Count() < 420)
    {
        // Clear existing movies to avoid duplicates
        context.Movies.RemoveRange(context.Movies.ToList());
        context.SaveChanges();

        // Build a dictionary mapping category name -> CategoryId for fast lookup during import
        var categoryLookup = context.Categories
            .ToDictionary(c => c.Name, c => c.CategoryId, StringComparer.OrdinalIgnoreCase);

        // Configure CsvHelper to be lenient with missing/extra fields
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            HeaderValidated = null
        };

        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, config);
        var rows = csv.GetRecords<MovieCsvRow>().ToList();

        foreach (var row in rows)
        {
            // Parse year: extract the first four-digit year from ranges like "2001-2002"
            var yearStr = row.Year?.Trim() ?? "";
            int year = 1888;
            if (yearStr.Length >= 4)
            {
                var firstPart = yearStr.Contains('-') ? yearStr[..yearStr.IndexOf('-')].Trim() : yearStr;
                if (int.TryParse(firstPart.Length >= 4 ? firstPart[..4] : firstPart, out var y) && y >= 1888 && y <= 2100)
                    year = y;
            }

            var edited = string.Equals(row.Edited, "Yes", StringComparison.OrdinalIgnoreCase) ? (bool?)true : null;
            var notes = row.Notes != null && row.Notes.Length > 25 ? row.Notes[..25] : row.Notes;

            // Look up the CategoryId from the seeded categories; default to "Miscellaneous" if not found
            var categoryName = row.Category?.Trim() ?? "Miscellaneous";
            int categoryId = categoryLookup.ContainsKey(categoryName)
                ? categoryLookup[categoryName]
                : categoryLookup["Miscellaneous"];

            context.Movies.Add(new Movie
            {
                Id = row.Id,
                CategoryId = categoryId,
                Title = row.Title?.Trim() ?? "",
                Year = year,
                Director = row.Director?.Trim() ?? "",
                Rating = row.Rating?.Trim() ?? "",
                Edited = edited,
                LentTo = string.IsNullOrWhiteSpace(row.LentTo) ? null : row.LentTo.Trim(),
                Notes = string.IsNullOrWhiteSpace(notes) ? null : notes
            });
        }

        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

// Default route: {controller=Home}/{action=Index}/{id?}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
