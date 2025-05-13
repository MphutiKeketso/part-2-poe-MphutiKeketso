using Microsoft.EntityFrameworkCore;
using EventEaseBookingSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Hardcoded connection string (directly in the code)
var connectionString = "Server=tcp:cldvst10445280.database.windows.net,1433;Initial Catalog=EventEase;Persist Security Info=False;User ID=ST10445280;Password=Keketso12@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext to the service container with the hardcoded connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));  // Use hardcoded connection string here

// You can configure other services here, such as Identity, Authentication, etc.

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
