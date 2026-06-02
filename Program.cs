using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Aklasa.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Aklasa.Models;
using Aklasa.Helpers;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PilkaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PilkaContext") ?? throw new InvalidOperationException("Connection string 'PilkaContext' not found.")));


builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

// Add services to the container.
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Druzyny}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider.GetRequiredService<PilkaContext>();

    if (!context.Uzytkownik.Any())
    {
        context.Uzytkownik.Add(
            new Uzytkownik
            {
                Login = "admin",
                HasloHash =
                    PasswordHelper.HashPassword("admin123"),

                CzyAdministrator = true
            });

        context.SaveChanges();
    }
}


app.Run();
