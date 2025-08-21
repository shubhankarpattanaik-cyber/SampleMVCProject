var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Get port from environment variable (Elastic Beanstalk sets this)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

// Configure Kestrel / URLs
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

app.MapGet("/", () => "Hello from Elastic Beanstalk!");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Register}/{id?}");


app.Run();
