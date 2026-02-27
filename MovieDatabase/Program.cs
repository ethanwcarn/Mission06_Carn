// Program.cs - Entry point for the Joel Hilton Movie Database ASP.NET Core MVC application.
// This file configures services, middleware, and routing for the web application.

using Microsoft.EntityFrameworkCore;
using MovieDatabase.Models;

// Create the web application builder, which sets up configuration, logging, and DI
var builder = WebApplication.CreateBuilder(args);

// Register MVC services so the app can use controllers and Razor views
builder.Services.AddControllersWithViews();

// Register the MovieContext with dependency injection, using SQLite as the database provider.
// The connection string "MovieConnection" is read from appsettings.json.
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MovieConnection")));

// Configure Kestrel web server to listen on specific ports for HTTPS and HTTP
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(7297, listenOptions => listenOptions.UseHttps());
    options.ListenLocalhost(5298);
});

// Build the configured application
var app = builder.Build();

// Configure the HTTP request pipeline (middleware).
// In non-development environments, use a generic error handler and enable HSTS for security.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS for security
app.UseHttpsRedirection();

// Enable routing so the app can match URLs to controller actions
app.UseRouting();

// Enable authorization middleware (required even if no auth policies are configured)
app.UseAuthorization();

// Serve static files from wwwroot (CSS, JS, images, libraries)
app.MapStaticAssets();

// Set up the default MVC routing pattern: {controller}/{action}/{id?}
// Defaults to HomeController.Index if no route segments are provided
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Start the application and begin listening for HTTP requests
app.Run();
