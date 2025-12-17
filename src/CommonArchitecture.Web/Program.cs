using CommonArchitecture.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register HttpClient for API communication
builder.Services.AddHttpClient<CommonArchitecture.Web.Services.IProductApiService, CommonArchitecture.Web.Services.ProductApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5089");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<CommonArchitecture.Web.Services.IRoleApiService, CommonArchitecture.Web.Services.RoleApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5089");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<CommonArchitecture.Web.Services.IUserApiService, CommonArchitecture.Web.Services.UserApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5089");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<CommonArchitecture.Web.Services.IAuthApiService, CommonArchitecture.Web.Services.AuthApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5089");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Add Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CommonArchitecture.Web.Validators.CreateProductDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CommonArchitecture.Web.Validators.CreateRoleDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CommonArchitecture.Web.Validators.CreateUserDtoValidator>();

// Register NToastNotify with ControllersWithViews
builder.Services.AddControllersWithViews().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000,
    Theme = "metroui"
});

var app = builder.Build();

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

// Add Session middleware
app.UseSession();

app.UseAuthorization();

// Add NToastNotify middleware
app.UseNToastNotify();

app.MapStaticAssets();

// Area route mapping - must come before default route
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
