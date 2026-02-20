using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure EF Core with SQLite
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MovieConnection")));

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(7297, listenOptions => listenOptions.UseHttps());
    options.ListenLocalhost(5298); // use different HTTP port
});
var app = builder.Build();

// One-time import from Joel Hilton CSV when database has few movies (e.g. legacy seed).
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    var csvPath = Path.Combine(env.ContentRootPath, "Data", "JoelHiltonMovieCollection.csv");

    // Import CSV when the collection is smaller than the full list (so first run or after manual adds).
    if (File.Exists(csvPath) && context.Movies.Count() < 420)
    {
        context.Movies.RemoveRange(context.Movies.ToList());
        context.SaveChanges();

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
            // Parse year: use first four-digit year if range (e.g. "2001-2002" -> 2001).
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

            context.Movies.Add(new Movie
            {
                Id = row.Id,
                Category = row.Category?.Trim() ?? "",
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
