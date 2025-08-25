using NToastNotify;
using SimpleBank.Client.Middleware;
using SimpleBank.Client.Repository.Implementation;
using SimpleBank.Client.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IGenericHttpClients, GenericHttpClients>();

builder.Services.AddAuthentication("Cookies") // Specify the scheme
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        // You can configure other options as needed
    });

builder.Services.AddControllersWithViews().AddNToastNotifyToastr(new ToastrOptions
{
    TimeOut = 2000,
    NewestOnTop = false,
    CloseButton = true,
    CloseDuration = true,
    ProgressBar = false,
    PositionClass = ToastPositions.TopRight,
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Custom Middleware
app.UseErrorHandlingMiddleware();

//app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
