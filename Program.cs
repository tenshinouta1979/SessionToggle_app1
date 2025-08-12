// Program.cs (for App1)
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor(); // Needed to access HttpContext in services if required, good practice for auth scenarios

// Configure and Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set a timeout for demonstration
    options.Cookie.HttpOnly = true;                 // Session cookie accessible only via HTTP requests
    options.Cookie.IsEssential = true;              // Make the session cookie essential
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Use session middleware (must be before UseAuthentication/UseAuthorization)
app.UseAuthentication(); // If you had actual authentication schemes (e.g., cookie auth)
app.UseAuthorization();

app.MapRazorPages(); // Maps incoming requests to Razor Pages

app.Run();
