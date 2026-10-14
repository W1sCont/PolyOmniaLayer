using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PolyOmnia.BLL.Infrastructure;
using PolyOmniaLayer;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
if (connection != null)
    builder.Services.AddPolyOmniaContext(connection);

// Локалізація
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Додаємо MVC з підтримкою локалізації View та DataAnnotations
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

// Конфігурація мов
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var langSection = builder.Configuration.GetSection("Lang");
    var cultureCodes = langSection.GetChildren().Select(c => c.Key).ToArray();

    var supportedCultures = cultureCodes
        .Select(code => new CultureInfo(code))
        .ToList();
    if (!supportedCultures.Any())
    {
        supportedCultures.Add(new CultureInfo("uk"));
    }

    options.DefaultRequestCulture = new RequestCulture("uk");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Сесії та Кеш
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// DI
builder.Services.AddUnitOfWorkService();
builder.Services.AddPolyOmniaServices();
builder.Services.AddPolyOmniaAutoMapper();

builder.Services.AddControllersWithViews();

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

// app.UseRequestLocalization();
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
